using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCPathing : MonoBehaviour
{
    
    public Transform path;
    public float motorTorque = 2000f;
    public float maxSteer = 40f;
    public float maxSpeed = 10f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;
    public int currentNode = 0;
    public GameObject startPos;

    private List<Transform> nodes;    
    private Rigidbody rigidBody;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++)
        {
            if(pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            rigidBody.transform.position = startPos.transform.position;
            rigidBody.transform.rotation = startPos.transform.rotation;
            rigidBody.linearVelocity = new Vector3(0,0,0);
            // Debug.Log("reset NPCcar pos");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplySteer();
        // Calculate current speed along the car's forward axis
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed)); // Normalized speed factor

        // Reduce motor torque and steering at high speeds for better handling
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        

        //slow down when nearing currentNode at high speeds
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        if(relativeVector.x < 1.5 && forwardSpeed > maxSpeed*.7)
        {
            // print("Speed " + forwardSpeed + " too high, slowing down");
            
            wheelBL.brakeTorque = 5000f;
            wheelBL.brakeTorque = 5000f;
            wheelBL.motorTorque = 0f;
            wheelBR.motorTorque = 0f;
        } else
        {
            wheelBL.brakeTorque = 0f;
            wheelBR.brakeTorque = 0f;
            wheelBL.motorTorque = currentMotorTorque;
            wheelBR.motorTorque = currentMotorTorque;
        }

    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        // print("Targeted Node: " + nodes[currentNode].name + " Relative Vector: " + relativeVector);

        //switch to next node when passing currentNode
        if(relativeVector.z < 0 && relativeVector.x < 1)
        {
            currentNode++;
            if(currentNode == nodes.Count)
            {
                currentNode = 0;
            }
            relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        }

        //turn towards currentNode
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }
}
