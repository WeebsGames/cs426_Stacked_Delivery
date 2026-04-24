using Unity.Mathematics;
using UnityEngine;

public class DriftScorer : MonoBehaviour
{
    public WheelCollider wheel;
    public ScoreManager scoreManager;

    WheelHit hit;

    void Update()
    {
        if (wheel.GetGroundHit(out hit))
        {
            if (math.abs(hit.sidewaysSlip) > 0.2f)
            {
                scoreManager.driftPoints += 10f * Time.deltaTime;
            }
        }
    }
}
