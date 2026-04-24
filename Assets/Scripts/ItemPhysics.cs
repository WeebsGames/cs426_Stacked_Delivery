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

    public AudioSource source;

    public LevelEnd levelEnd;
    public LevelTimer levelTimer;

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
            // Debug.Log($"Stability: {stability:F1}% | Lateral G: {lateralG:F2}");
            float rotationSpeed = carRigidbody.angularVelocity.magnitude;
            // Debug.Log($"Stability: {stability:F1}% | Lateral G: {lateralG:F2} | Rotation: {rotationSpeed:F2}");
        }
    }

    //
    // calculateLateralGForce()
    //
    // This method calculates sideways forces from drifting/turning
    // Goal: Sideways movement -> items fall off
    // Calculate force by using flat plane by using sideways
    // axis of car, removing the tilt from hills/slopes
    // Dot gives +/- or 0, and get abs value of lateral force for both
    // sides
    //
    float CalculateLateralGForce()
    {
        Vector3 worldVel = carRigidbody.linearVelocity;
        Vector3 carRight = transform.right;
        carRight.y = 0;
        carRight.Normalize();

        float lateralG = Vector3.Dot(worldVel, carRight);

        return Mathf.Abs(lateralG);
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

        stability = Mathf.Clamp(stability, 0f, 100f);

        // Console log to track stability state
        if (stability < 25f && stability > 0f)
        {
            // Debug.LogWarning("STABILITY CRITICAL!");
        }

        if (stability <= 0f)
        {
            // Debug.LogError("CARGO FELL OFF!");
        }

        if (stability <= 0f && !itemsFallen)
        {
            DropOneItem();
        }


        if (lateralG < smoothDrivingThreshold && rotationSpeed < 1.5f)
        {
            stability += recoveryRate * Time.fixedDeltaTime;
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
        // Get velocity and get car's sideways axis flat to ground
        // and measure sideways velocity on flat plane when going up
        // slope/hill
        Vector3 worldVel = carRigidbody.linearVelocity;
        Vector3 carRight = transform.right;
        carRight.y = 0;
        carRight.Normalize();

        float lateralVel = Vector3.Dot(worldVel, carRight);
        // negative to tilt stack opposite direction of movement
        // 7f just for visual effect of tilt applied
        float targetTiltZ = -lateralVel * 7f;

        // stop tilt from tilting crazy
        targetTiltZ = Mathf.Clamp(targetTiltZ, -maxTiltAngle, maxTiltAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTiltZ);
        itemStack.localRotation = Quaternion.Slerp(
            itemStack.localRotation,
            targetRotation,
            Time.fixedDeltaTime * 5f
        );

        // wobble when unstable
        if (stability < 25f)
        {
            float wobbleAmount = (25f - stability) / 25f;
            float wobble = Mathf.Sin(Time.time * 12f) * wobbleAmount * 10f;
            itemStack.localRotation *= Quaternion.Euler(0, 0, wobble);
        }
    }

    //
    // DropOneItem()
    //
    // When the stability value reaches 0 it will remove the
    // last item from the list/stack and apply a random force 
    // to that item to simulate it flying out the stack
    //
    void DropOneItem()
    {
        //itemsFallen = true;
        // Debug.LogError("Falling items now");
        if (itemBoxes.Count == 0) return;

        source.Play();

        // remove the last item in the list/stack
        int lastIndex = itemBoxes.Count - 1;
        GameObject item = itemBoxes[lastIndex];
        itemBoxes.RemoveAt(lastIndex);

        // add a box collider and rigidbody
        item.transform.SetParent(null);
        item.AddComponent<BoxCollider>();
        Rigidbody itemRB = item.AddComponent<Rigidbody>();

        // set a random force value to have the item fly off
        Vector3 randomForce = new Vector3(
            Random.Range(-1, 1f),
            Random.Range(0.5f, 1f),
            Random.Range(-1f, 1f)
        ) * fallForce;

        // apply the force
        itemRB.AddForce(randomForce, ForceMode.Impulse);
        itemRB.AddTorque(Random.insideUnitSphere * fallForce, ForceMode.Impulse);

        if (itemBoxes.Count == 0)
        {
            Invoke("TriggerLoseCargo", 2f);
            return;
        }

        // reset stability after one item has fallen
        stability = 100f;
        itemsFallen = false;
    }

    // Called after a 2 second delay to show level end panel and to
    // stop timer.
    void TriggerLoseCargo()
    {
        if (levelTimer != null) levelTimer.StopTimer();
        if (levelEnd != null) levelEnd.LoseCargo();
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

        // ignore everything with the ground tag, all of the Road
        // System child objects essentially
        if (collision.gameObject.CompareTag("ground"))
        {
            return;
        }

        float impactForce = collision.impulse.magnitude;
        // print("Impact force " + impactForce);

        if (impactForce > impactThreshold)
        {
            float damage = impactForce * impactMultiplier;
            stability -= damage;
            stability = Mathf.Clamp(stability, 0f, 100f);
            // Debug.Log($"Collision with {collision.gameObject.name} | Impact: {impactForce:F1} | Damage: {damage:F1} | Stability: {stability:F1}%");
        }
    }

    //
    // GetStability()
    //
    // Returns current stability as a value from 0 to 1.
    // 0 = completely unstable, 1 = perfectly stable.
    public float GetStability()
    {
        return stability / 100f;
    }

    public bool GetItemsFallen()
    {
        return itemsFallen;
    }
}