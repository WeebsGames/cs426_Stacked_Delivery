using UnityEngine;

//
// ItemPhysics to simulation stacked items on top of vehicle
//
public class ItemPhysics : MonoBehaviour
{
    [Header("Stack State")]
    public float stability = 100f;

    [Header("Tuning")]
    [Range(0f, 20f)]
    public float lateralGMultiplier = 50f;
    [Range(0f, 10f)]
    public float recoveryRate = 2f;

    [Header("Visual Stack")]
    public Transform itemStack;
    [Range(0f, 30f)]
    public float maxTiltAngle = 15f;

    // assuming car has rigidbody component
    private Rigidbody carRigidbody;

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
        float lateralG = lateralSpeed / 10f;
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
        float targetTilt = -localVel.x * 2f;

        targetTilt = Mathf.Clamp(targetTilt, -maxTiltAngle, maxTiltAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);
        itemStack.localRotation = Quaternion.Slerp(
            itemStack.localRotation,
            targetRotation,
            Time.fixedDeltaTime * 5f
        );

        if (stability < 25f)
        {
            float wobbleAmount = (25f - stability) / 25f;
            float wobble = Mathf.Sin(Time.time * 10f) * wobbleAmount * 5f;
            itemStack.localRotation *= Quaternion.Euler(0, 0, wobble);
        }
    }
}