using UnityEngine;

public class finishscirpt : MonoBehaviour
{

    public AudioSource finishSound;
    public AudioClip finishClip;
    public LevelTimer levelTimer;
    public LevelEnd levelEnd;
    public ItemPhysics itemPhysics;

    void Start()
    {
        levelEnd = FindAnyObjectByType<LevelEnd>();
        levelTimer = FindAnyObjectByType<LevelTimer>();
    }

    public void FindCar()
    {
        itemPhysics = GameObject.FindWithTag("Player").GetComponent<ItemPhysics>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
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
