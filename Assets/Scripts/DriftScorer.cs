using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DriftScorer : MonoBehaviour
{
    public WheelCollider wheel;
    public ScoreManager scoreManager;
    public TextMeshProUGUI driftPointsText;

    WheelHit hit;

    void Update()
    {
        if (wheel.GetGroundHit(out hit))
        {
            if (math.abs(hit.sidewaysSlip) > 0.2f)
            {
                scoreManager.driftPoints += 10f * Time.deltaTime;
                driftPointsText.gameObject.SetActive(true);
                driftPointsText.text = "Drift Points: " + Mathf.RoundToInt(scoreManager.driftPoints);
            }
        }
    }
}
