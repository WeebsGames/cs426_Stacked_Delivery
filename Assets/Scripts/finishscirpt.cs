using UnityEngine;

public class finishscirpt : MonoBehaviour
{

    public AudioSource finishSound;
    public AudioClip finishClip;
    public LevelTimer levelTimer;
    public LevelEnd levelEnd;
    public ItemPhysics itemPhysics;
    public ScoreManager scoreManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("finish");
            finishSound.Stop();
            finishSound.clip = finishClip;
            finishSound.Play();

            if (levelTimer != null)
            {
                levelTimer.StopTimer();
            }

            if (levelEnd != null)
            {
                if (itemPhysics != null && itemPhysics.GetItemsFallen())
                {
                    levelEnd.LoseCargo();
                }
                else
                {
                    // add points to time and cargo
                    // time bonus points for time remaining on timer
                    // item points bonus for number of items * 100
                    scoreManager.timeBonus = levelTimer.GetTimeRemaining();
                    scoreManager.cargoBonus = itemPhysics.itemBoxes.Count * 100f;
                    levelEnd.Win();
                }
            }

            return;
        }
    }
}
