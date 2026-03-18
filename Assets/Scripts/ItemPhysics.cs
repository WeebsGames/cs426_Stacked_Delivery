using UnityEngine;

//
// ItemPhysics to simulate stacked items on top of vehicle
//
public class ItemPhysics : MonoBehaviour
{
    [Header("Stack State")]
    public float stability = 100f;

    [Header("Tuning")]
    [Range(0f, 100f)]
    public float lateralGMultiplier = 10f;
    [Range(0f, 100f)]
    public float recoveryRate = 40f;

    [Range(0f, 50f)]
    public float angularVelocityMultiplier = 2f;
    public float smoothDrivingThreshold = 2f;

    [Header("Visual Stack")]
    public Transform itemStack;
    [Range(0f, 100f)]
    public float maxTiltAngle = 50f;

    [Header("Falling Items settings")]
    public System.Collections.Generic.List<GameObject> itemBoxes;
    public float fallForce = 5f;

    [Header("Collision settings")]
    [Range(0f, 50f)]
    public float impactMultiplier = 5f;
    public float impactThreshold = 8f;

    // assuming car has rigidbody component
    private Rigidbody carRigidbody;
    private bool itemsFallen = false;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();

        if (carRigidbody == null)
        {
            Debug.LogError("No Rigidbody found on vehicle");
        }
        else
        {
            Debug.Log("rigidbody on the vehicle");
        }
    }

    void FixedUpdate()
    {
        if (carRigidbody == null) return;
        if (itemsFallen) return;

        float lateralG = CalculateLateralGForce();

        UpdateStability(lateralG);

        if (itemStack != null)
        {
            UpdateStackVisual(lateralG);
        }

        // This is just to showcase a stability tracker on the console
        if (Time.frameCount % 30 == 0)
        {
            Debug.Log($"Stability: {stability:F1}% | Lateral G: {lateralG:F2}");
            float rotationSpeed = carRigidbody.angularVelocity.magnitude;
            Debug.Log($"Stability: {stability:F1}% | Lateral G: {lateralG:F2} | Rotation: {rotationSpeed:F2}");
        }
    }

    //
    // calculateLateralGForce()
    //
    // This method calculates sideways forces from drifting/turning
    // Goal: Sideways movement -> items slide off
    //
    float CalculateLateralGForce()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        float lateralG = Mathf.Abs(localVelocity.x);
        return lateralG;
    }

    //
    // UpdateStability()
    //
    // This method modifies stability based on driving behavior.
    // Aggressive driving decreases stability, smooth driving recovers 
    // it.
    //
    void UpdateStability(float lateralG)
    {
        float rotationSpeed = carRigidbody.angularVelocity.magnitude;
        float stabilityLoss = (lateralG * lateralGMultiplier) + (rotationSpeed * angularVelocityMultiplier);

        stability -= stabilityLoss * Time.fixedDeltaTime;

        if (lateralG < smoothDrivingThreshold && rotationSpeed < 1.5f)
        {
            stability += recoveryRate * Time.fixedDeltaTime;
        }

        stability = Mathf.Clamp(stability, 0f, 100f);

        // Console log to track stability state
        if (stability < 25f && stability > 0f)
        {
            Debug.LogWarning("STABILITY CRITICAL!");
        }

        if (stability <= 0f)
        {
            Debug.LogError("CARGO FELL OFF!");
        }

        if (stability <= 0f && !itemsFallen)
        {
            MakeItemsFall();
        }
    }

    //
    // UpdateStackVisual()
    //
    // This method makes item stack visibly tilt and wobble based on 
    // forces. Wobble intensity increases as stability decreases to
    // simulate unbalance of the items. 
    //
    //
    void UpdateStackVisual(float lateralG)
    {
        Vector3 localVel = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        float targetTiltZ = -localVel.x * 10f;

        targetTiltZ = Mathf.Clamp(targetTiltZ, -maxTiltAngle, maxTiltAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTiltZ);
        itemStack.localRotation = Quaternion.Slerp(
            itemStack.localRotation,
            targetRotation,
            Time.fixedDeltaTime * 5f
        );

        if (stability < 25f)
        {
            float wobbleAmount = (25f - stability) / 25f;
            float wobble = Mathf.Sin(Time.time * 12f) * wobbleAmount * 10f;
            itemStack.localRotation *= Quaternion.Euler(0, 0, wobble);
        }
    }

    //
    // MakeItemsFall()
    //
    // This method handles when the stability value reaches 0 and the
    // items haven't fallen, then it will apply a random force to
    // all the items in the stack, to simulate the items flying out
    // the stack due to reckless driving or collision
    //
    //
    void MakeItemsFall()
    {
        itemsFallen = true;
        Debug.LogError("Falling items now");

        foreach (GameObject box in itemBoxes)
        {
            if (box == null) continue;

            box.transform.SetParent(null);

            Rigidbody boxRB = box.AddComponent<Rigidbody>();

            Vector3 randomForce = new Vector3(
                Random.Range(-1, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(-1f, 1f)
            ) * fallForce;

            boxRB.AddForce(randomForce, ForceMode.Impulse);

            boxRB.AddTorque(Random.insideUnitSphere * fallForce, ForceMode.Impulse);
        }
    }

    //
    // OnCollisionEnter()
    //
    // This method detects collisions with walls/barriers and damages
    // stability. Harder impact -> more stability loss
    //
    void OnCollisionEnter(Collision collision)
    {
        if (itemsFallen) return;

        if (collision.gameObject.name == "Floor")
        {
            return;
        }

        float impactForce = collision.impulse.magnitude;

        if (impactForce > impactThreshold)
        {
            float damage = impactForce * impactMultiplier;
            stability -= damage;
            stability = Mathf.Clamp(stability, 0f, 100f);
            Debug.Log($"Collision, Impact force: {impactForce:F1} | Damage: {damage:F1} | Stability: {stability:F1}%");
        }
    }
}