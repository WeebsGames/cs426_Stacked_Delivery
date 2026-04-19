using UnityEngine;

// Makes the car slippery by lowering wheel friction while over trigger.
public class SlipperySurface : MonoBehaviour
{
    public float slipperyFriction = 0.05f;
    public float normalFriction = 1f;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        if (!rb.CompareTag("Player")) return;

        SetFriction(rb, slipperyFriction);
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        if (!rb.CompareTag("Player")) return;

        SetFriction(rb, normalFriction);
    }

    // Set the friction on all wheels of the car.
    void SetFriction(Rigidbody car, float value)
    {
        WheelCollider[] wheels = car.GetComponentsInChildren<WheelCollider>();

        foreach (WheelCollider wheel in wheels)
        {
            WheelFrictionCurve sideways = wheel.sidewaysFriction;
            WheelFrictionCurve forward = wheel.forwardFriction;

            sideways.stiffness = value;
            forward.stiffness = value;

            wheel.sidewaysFriction = sideways;
            wheel.forwardFriction = forward;
        }
    }
}