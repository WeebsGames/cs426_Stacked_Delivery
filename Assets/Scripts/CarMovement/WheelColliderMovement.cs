using UnityEngine;
using UnityEngine.InputSystem;

//
// WheelColliderMovement
//
// Movement system using Unity's WheelColliders for real physics suspension.
// Backup option to visual suspension system.
//
public class WheelColliderMovement : MonoBehaviour
{
    [Header("Wheel Collider References")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Movement Settings")]
    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 30f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float motor = 0f;
        float brake = 0f;
        float steer = 0f;

        // Forward
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            motor = motorForce;
        }

        // Brake or Reverse
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            if (rb.linearVelocity.magnitude > 0.5f && Vector3.Dot(rb.linearVelocity, transform.forward) > 0)
            {
                brake = brakeForce;
            }
            else
            {
                motor = -motorForce;
            }
        }

        // Steering
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            steer = -maxSteerAngle;
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            steer = maxSteerAngle;
        }

        wheelFL.motorTorque = motor;
        wheelFR.motorTorque = motor;
        wheelRL.motorTorque = motor;
        wheelRR.motorTorque = motor;

        wheelFL.brakeTorque = brake;
        wheelFR.brakeTorque = brake;
        wheelRL.brakeTorque = brake;
        wheelRR.brakeTorque = brake;

        // Only front wheels steer
        wheelFL.steerAngle = steer;
        wheelFR.steerAngle = steer;
    }
}