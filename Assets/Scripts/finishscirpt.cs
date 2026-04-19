using UnityEngine;

public class finishscirpt : MonoBehaviour
{

    public AudioSource finishSound;
    public AudioClip finishClip;
    public LevelTimer levelTimer;
    public LevelEnd levelEnd;
    public ItemPhysics itemPhysics;

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
                    levelEnd.Win();
                }
            }

            return;
        }
    }
}
