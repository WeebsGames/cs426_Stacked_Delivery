using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCPathing : MonoBehaviour
{
    
    public Transform path;
    public float maxSteer = 40;
    public float carSpeed = 20f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    private List<Transform> nodes;
    private int currentNode = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplySteer();
        wheelBL.motorTorque = carSpeed;
        wheelBR.motorTorque = carSpeed;
    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }
}
