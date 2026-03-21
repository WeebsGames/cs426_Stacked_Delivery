using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{

    public float steeringRadius = 30.0f;
    public float acceleration = 1f;
    public float maxSpeed = 10f;

    float speed;
    int turn;
    int collisions;
    float vel;
    bool grounded = true;
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
        float speed = rb.linearVelocity.magnitude * 2;

        if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        { 
            if(speed < maxSpeed)
            {
                vel += acceleration * 1-(speed/maxSpeed);
            }
        } else
        {
            if(vel > 0)
            {
                vel -= acceleration * 5;
            }
        }

        //braking input
        if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            if(vel > 0)
            {
                vel -= acceleration * 20;
            } else
            {
                vel = 0;
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

        //flip car
        if (Keyboard.current.fKey.wasReleasedThisFrame && collisions > 0)
        {
            t.position += new Vector3(0,10,0);
            t.eulerAngles = new Vector3(0,0,0);
        }

        //linear velocity formula
        // float ke = 0.5f * rb.mass * (vel * vel); //--velocity kept increasing exponentially
        float ke = math.sqrt(math.abs(vel*2));
        // float ke = vel;
        //rotate car
        t.Rotate(0,steeringRadius*turn,0,Space.Self);

        dir = t.forward * ke;

        if (collisions > 0)
        {
            rb.linearVelocity += dir * Time.deltaTime;
        }        
        // print("time since last frame: " + Time.deltaTime);
        // print("kinetic energy: " + ke);
        // print("vel: " + vel);
        // print("speed: " + Math.Round(rb.linearVelocity.magnitude));


    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("collision detected with tag: " + collision.transform.tag);
        if(collision.transform.tag == "ground")
        {
            collisions++;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "ground")
        {
            collisions--;
        }
    }
}
