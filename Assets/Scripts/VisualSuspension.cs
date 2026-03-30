using UnityEngine;

//
// VisualSuspension
//
// Uses raycasts to detect ground and moves wheels meshes up and down
// to simulate suspension. Does not affect actual vehicle physics, it's
// just for visual effects of a suspension system of a car.
//
public class VisualSuspension : MonoBehaviour
{
    [Header("Wheel References")]
    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelRL;
    public Transform wheelRR;

    [Header("Suspension Settings")]
    [Range(0.1f, 2f)]
    public float suspensionTravel = 0.3f;
    [Range(1f, 20f)]
    public float suspensionStiffness = 10f;
    [Range(0.1f, 5f)]
    public float suspensionDamping = 2f;

    [Header("Raycast Settings")]
    public float raycastOffset = 0.3f;
    public float raycastDistance = 1f;
    public LayerMask groundLayer;

    private float[] wheelVelocities = new float[4];

    void Start()
    {
        if (groundLayer == 0)
        {
            groundLayer = ~0;
        }
    }

    void Update()
    {
        UpdateWheelSuspension(wheelFL, 0);
        UpdateWheelSuspension(wheelFR, 1);
        UpdateWheelSuspension(wheelRL, 2);
        UpdateWheelSuspension(wheelRR, 3);
    }

    //
    // UpdateWheelSuspension()
    //
    // This method updates a single wheel's position based on ground
    // distance. Raycasts points down to find ground, then moves wheel
    // mesh to simulate suspension compression.
    //
    void UpdateWheelSuspension(Transform wheelTransform, int wheelIndex)
    {
        if (wheelTransform == null) return;

        // Raycast down from wheel position
        RaycastHit hit;
        Vector3 rayOrigin = wheelTransform.position + Vector3.down * raycastOffset;

        if (Physics.Raycast(rayOrigin, -Vector3.up, out hit, raycastDistance, groundLayer))
        {
            float groundDistance = hit.distance;
            float compression = Mathf.Clamp(suspensionTravel - groundDistance, 0f, suspensionTravel);

            float targetY = -compression;

            float currentY = wheelTransform.localPosition.y;
            float newY = Mathf.SmoothDamp(
                currentY,
                targetY,
                ref wheelVelocities[wheelIndex],
                1f / suspensionStiffness,
                Mathf.Infinity,
                Time.deltaTime
            );

            wheelTransform.localPosition = new Vector3(
                wheelTransform.localPosition.x,
                newY,
                wheelTransform.localPosition.z
            );
        }

        else
        {
            float targetY = -suspensionTravel;
            float currentY = wheelTransform.localPosition.y;

            float newY = Mathf.SmoothDamp(
                currentY,
                targetY,
                ref wheelVelocities[wheelIndex],
                1f / suspensionStiffness,
                Mathf.Infinity,
                Time.deltaTime
            );

            wheelTransform.localPosition = new Vector3(
                wheelTransform.localPosition.x,
                newY,
                wheelTransform.localPosition.z
            );
        }
    }
}