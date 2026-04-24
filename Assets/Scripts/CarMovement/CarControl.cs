using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using NUnit.Framework;

public class CarControl : MonoBehaviour
{
    [Header("Car Properties")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;
    public int crashThreshold = 100;
    public AudioSource crashSource;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;
    private WheelFrictionCurve sideFric;
    private bool handbraking = false;
    private float prevSpeed;
    
    [Header("Input Actions")]
    public InputActionReference moveAction;

    [Header("OtherObjects")]
    // public TMP_Text speedText;
    public TMP_Text brakeText;
    public GameObject startPos;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass to improve stability and prevent rolling
        Vector3 centerOfMass = rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rigidBody.centerOfMass = centerOfMass;

        // Get all wheel components attached to the car
        wheels = GetComponentsInChildren<WheelControl>();

        if(brakeText == null)
        {
            brakeText = GameObject.Find("Handbrake Status").GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            rigidBody.transform.position = startPos.transform.position;
            rigidBody.transform.rotation = startPos.transform.rotation;
            rigidBody.linearVelocity = new Vector3(0,0,0);
            // Debug.Log("reset car pos");
        }

        //reduce friction of wheels to simulate handbraking
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handbraking = true;
            brakeText.text = "Handbrake: " + handbraking;
            print("Handbraking: " + handbraking);
            foreach(var wheel in wheels)
            {
                if (wheel.motorized)
                {
                    sideFric = wheel.WheelCollider.sidewaysFriction;
                    if (handbraking)
                    {
                        sideFric.stiffness = 0.4f;
                    } else
                    {
                        sideFric.stiffness = 1f;
                    }
                    wheel.WheelCollider.sidewaysFriction = sideFric;
                }
            }

            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            handbraking = false;
            brakeText.text = "Handbrake: " + handbraking;
            print("Handbraking: " + handbraking);
            foreach(var wheel in wheels)
            {
                if (wheel.motorized)
                {
                    sideFric = wheel.WheelCollider.sidewaysFriction;
                    if (handbraking)
                    {
                        sideFric.stiffness = 0f;
                    } else
                    {
                        sideFric.stiffness = 1f;
                    }
                    wheel.WheelCollider.sidewaysFriction = sideFric;
                }
            }
        }

        prevSpeed = rigidBody.linearVelocity.magnitude;
    }

    // FixedUpdate is called at a fixed time interval 
    void FixedUpdate()
    {
        // Get player input for acceleration and steering
        float vInput = moveAction.action.ReadValue<Vector2>().y; // Forward/backward input
        float hInput = moveAction.action.ReadValue<Vector2>().x; // Steering input

        // Calculate current speed along the car's forward axis
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed)); // Normalized speed factor

        //incentivise drifting
        if (handbraking)
        {
            speedFactor = Mathf.InverseLerp(0, maxSpeed * 1.5f, Mathf.Abs(forwardSpeed));
        }

        // Reduce motor torque and steering at high speeds for better handling
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Determine if the player is accelerating or trying to reverse
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);


        foreach (var wheel in wheels)
        {
            // Apply steering to wheels that support steering
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to motorized wheels
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                // Release brakes when accelerating
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                // Apply brakes when reversing direction
                if (wheel.motorized && !wheel.steerable)
                {
                    wheel.WheelCollider.motorTorque = 0f;
                    wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                }
            }
        }

        // speedText.text = "Speed: " + rigidBody.linearVelocity.magnitude;
    }

    void OnCollisionEnter(Collision collision)
    {
        // print("prevSpeed "+prevSpeed);
        if(prevSpeed > crashThreshold && rigidBody.linearVelocity.magnitude < 0.7 * prevSpeed)
        {
            // print("crashed");
            crashSource.time = 1.8f;
            crashSource.Play();
        }
    }

}
