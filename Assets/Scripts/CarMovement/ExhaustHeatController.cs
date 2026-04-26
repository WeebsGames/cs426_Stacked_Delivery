using UnityEngine;

// Drives the exhaust glow shader's heat intensity based on car speed.
// Heat builds up when driving fast and cools down when slow.
public class ExhaustHeatController : MonoBehaviour
{
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private float speedThreshold = 10f;
    [SerializeField] private float heatUpRate = 0.15f;
    [SerializeField] private float coolDownRate = 0.1f;

    private Material glowMaterial;
    private float currentHeat = 0f;

    // Get the glow material from this object's renderer.
    private void Start()
    {
        glowMaterial = GetComponent<MeshRenderer>().material;
    }

    // Build heat when fast, cool off when slow, then send value to the 
    // shader.
    private void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude;

        if (speed >= speedThreshold)
        {
            currentHeat += heatUpRate * Time.deltaTime;
        }
        else
        {
            currentHeat -= coolDownRate * Time.deltaTime;
        }

        currentHeat = Mathf.Clamp01(currentHeat);
        glowMaterial.SetFloat("_HeatIntensity", currentHeat);
    }
}