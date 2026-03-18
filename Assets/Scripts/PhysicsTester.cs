using UnityEngine;

// PhysicsTester is a simple script to test physics of
// a "vehicle" where the vehicle is a cube with forces applied
// when user inputs wasd keys for movement and spacebar for drifting
// Used for testing item physics behavior
public class PhysicsTester : MonoBehaviour
{
    private Rigidbody rb;
    public float forceAmount = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.right * forceAmount);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * forceAmount);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * forceAmount);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(Vector3.up * -forceAmount);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Vector3.up * forceAmount);
        }
    }
}