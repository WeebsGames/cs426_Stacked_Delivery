using UnityEngine;

//
// FSM that cycles a stoplight between color states
// based on player proximity using a box collider
// set to isTrigger
//
public class StoplightFSM : MonoBehaviour
{
    enum State { Green, Yellow, Red }

    private State currentState = State.Green;
    private float timer = 0f;

    [SerializeField] private float yellowDuration = 2f;
    [SerializeField] private float redDuration = 4f;

    [SerializeField] private Light redLight;
    [SerializeField] private Light yellowLight;
    [SerializeField] private Light greenLight;


    // Setting the initial state of the spotlight
    void Start()
    {
        SetState(State.Green);
    }

    // Handles state transitions based on how long
    // the current state has been active through timer
    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case State.Yellow:
                if (timer >= yellowDuration)
                    SetState(State.Red);
                break;

            case State.Red:
                if (timer >= redDuration)
                    SetState(State.Green);
                break;
        }
    }

    // Triggers yellow state when player enters the intersection
    // area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentState == State.Green)
            SetState(State.Yellow);
    }

    // Reset to green when player exits the intersection area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetState(State.Green);
    }

    // Apply new state by reset timer and updating light
    // visibility
    void SetState(State newState)
    {
        currentState = newState;
        timer = 0f;

        redLight.enabled = (newState == State.Red);
        yellowLight.enabled = (newState == State.Yellow);
        greenLight.enabled = (newState == State.Green);
    }
}
