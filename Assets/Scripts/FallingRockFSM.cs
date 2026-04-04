using UnityEngine;

public class FallingRockFSM : MonoBehaviour
{
    public enum RockState
    {
        Waiting,
        Warning,
        Falling,
        Cooldown
    }

    [Header("Scene References")]
    public Rigidbody rockRigidbody;
    public Transform rockVisual;
    public Transform resetPoint;

    [Header("Timing")]
    public float warningTime = 1.2f;
    public float resetDelay = 3f;

    [Header("Movement")]
    public Vector3 roadOffset = new Vector3(8f, -1f, -18f);
    public float moveSpeed = 10f;
    public float spinSpeed = 360f;
    public float arriveDistance = 0.2f;

    [Header("Debug")]
    public RockState currentState = RockState.Waiting;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 targetPosition;
    private bool playerInsideTrigger;
    private float stateTimer;

    void Start()
    {
        if (rockRigidbody == null)
        {
            rockRigidbody = GetComponentInChildren<Rigidbody>();
        }

        if (rockVisual == null && rockRigidbody != null)
        {
            rockVisual = rockRigidbody.transform;
        }

        if (resetPoint == null)
        {
            resetPoint = transform;
        }

        if (rockVisual != null)
        {
            startPosition = resetPoint.position;
            startRotation = resetPoint.rotation;
            targetPosition = startPosition + roadOffset;
        }

        ValidateSetup();
        ConfigureRockPhysics();

        PrepareRockForWaiting();
    }

    void Update()
    {
        switch (currentState)
        {
            case RockState.Waiting:
                break;

            case RockState.Warning:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    StartFalling();
                }
                break;

            case RockState.Falling:
                MoveRockTowardRoad();
                if (rockVisual != null && Vector3.Distance(rockVisual.position, targetPosition) <= arriveDistance)
                {
                    BeginCooldown();
                }
                break;

            case RockState.Cooldown:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                {
                    ResetRock();
                }
                break;
        }
    }

    public void NotifyPlayerEntered()
    {
        playerInsideTrigger = true;

        if (currentState != RockState.Waiting)
        {
            return;
        }

        currentState = RockState.Warning;
        stateTimer = warningTime;
        Debug.Log("Falling rock triggered: warning started.");
    }

    public void NotifyPlayerExited()
    {
        playerInsideTrigger = false;
    }

    void StartFalling()
    {
        currentState = RockState.Falling;
        Debug.Log("Falling rock started falling.");
    }

    void BeginCooldown()
    {
        currentState = RockState.Cooldown;
        stateTimer = resetDelay;
    }

    void ResetRock()
    {
        if (rockVisual != null)
        {
            rockVisual.position = startPosition;
            rockVisual.rotation = startRotation;
        }

        PrepareRockForWaiting();

        if (playerInsideTrigger)
        {
            currentState = RockState.Warning;
            stateTimer = warningTime;
        }
    }

    void PrepareRockForWaiting()
    {
        currentState = RockState.Waiting;

        if (rockRigidbody == null)
        {
            return;
        }

        rockRigidbody.useGravity = false;
        rockRigidbody.isKinematic = true;
    }

    void ValidateSetup()
    {
        if (rockRigidbody == null)
        {
            Debug.LogError("FallingRockFSM: Rock Rigidbody is missing.", this);
        }

        if (rockVisual == null)
        {
            Debug.LogError("FallingRockFSM: Rock Visual is missing.", this);
        }

        if (resetPoint == null)
        {
            Debug.LogError("FallingRockFSM: Reset Point is missing.", this);
        }
    }

    void ConfigureRockPhysics()
    {
        if (rockRigidbody == null)
        {
            return;
        }

        rockRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rockRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rockRigidbody.useGravity = false;
        rockRigidbody.isKinematic = true;
    }

    void MoveRockTowardRoad()
    {
        if (rockVisual == null)
        {
            return;
        }

        rockVisual.position = Vector3.MoveTowards(
            rockVisual.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        Vector3 moveDirection = (targetPosition - rockVisual.position).normalized;
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            rockVisual.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}
