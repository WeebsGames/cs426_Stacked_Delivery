using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float driftPoints;
    public float timeBonus;
    public float cargoBonus;

    public float GetTotal()
    {
        return driftPoints + timeBonus + cargoBonus;
    }

    public void Reset()
    {
        driftPoints = 0;
        timeBonus = 0;
        cargoBonus = 0;
    }
}
