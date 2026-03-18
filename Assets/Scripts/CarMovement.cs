using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{

    public float steeringRadius = 30.0f;
    public float acceleration = 1f;
    public float maxSpeed = 10f;

    float speed;
    int turn;
    float vel;
    Rigidbody rb;
    Transform t;
    Vector3 dir = new Vector3(0,0,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //forward acceleration input
        if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            vel += acceleration;
            if(vel > maxSpeed)
            {
                vel = maxSpeed;
            }
        } else
        {
            if(vel > 0)
            {
                vel -= acceleration;
            }
        }

        //braking input
        if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            if(vel > 0)
            {
                vel -= acceleration;
            }
        }

        //turning input
        if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            turn = -1;
        } else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) {
            turn = 1;
        } else
        {
            turn = 0;
        }

        //linear velocity formula
        float ke = 0.5f * rb.mass * (vel * vel);

        //rotate car
        t.Rotate(0,steeringRadius*turn,0,Space.Self);

        dir = t.forward * ke;

        rb.linearVelocity = dir;
        print("direction vector: " + dir);
        print("kinetic energy: " + ke);
        print("vel: " + vel);


    }
}
