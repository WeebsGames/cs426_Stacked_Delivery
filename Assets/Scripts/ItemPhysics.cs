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
    public float lateralGMultiplier = 50f;
    [Range(0f, 10f)]
    public float recoveryRate = 2f;

    [Header("Visual Stack")]
    public Transform itemStack;
    [Range(0f, 100f)]
    public float maxTiltAngle = 90f;

    [Header("Falling Items settings")]
    public System.Collections.Generic.List<GameObject> itemBoxes;
    public float fallForce = 5f;
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
        float lateralSpeed = Mathf.Abs(localVelocity.x);

        // if (Input.GetKey(KeyCode.Space))
        // {
        //     Debug.Log($"Raw Lateral Speed: {lateralSpeed:F2} | After /10: {lateralSpeed / 10f:F2}");
        // }

        float lateralG = lateralSpeed;
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
        float stabilityLoss = lateralG * lateralGMultiplier;

        stability -= stabilityLoss * Time.fixedDeltaTime;

        if (lateralG < 0.3f)
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
}