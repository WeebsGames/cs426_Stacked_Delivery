using UnityEngine;

//
//Speedometer uses current speed of carcontrol, set to 20f
// added inspector fields to change value if needed
//
public class SpeedometerUI : MonoBehaviour
{
    [Header("References")]
    public RectTransform needle;
    public Rigidbody carRigidbody;

    [Header("Needle Settings")]
    public float minAngle = 217f;
    public float maxAngle = -40f;
    public float maxSpeed = 20f;

    void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minAngle, maxAngle, t);

        needle.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}
