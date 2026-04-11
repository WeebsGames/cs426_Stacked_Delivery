using UnityEngine;

public class crashController : MonoBehaviour
{
    public int crashThreshold = 100;
    public AudioSource crashSource;
    public Rigidbody rb;
    
    private float prevSpeed;
    // Update is called once per frame
    void Update()
    {
        prevSpeed = rb.linearVelocity.magnitude;
    }

    void OnCollisionEnter(Collision collision)
    {
        print("prevSpeed "+prevSpeed);
        if(prevSpeed > crashThreshold)
        {
            crashSource.Stop();
            crashSource.time = 5.0f;
            crashSource.Play();
        }
    }
}
