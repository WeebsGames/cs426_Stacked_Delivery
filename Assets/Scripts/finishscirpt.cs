using UnityEngine;

public class finishscirpt : MonoBehaviour
{

    public AudioSource finishSound;
    public AudioClip finishClip;
    public LevelTimer levelTimer;
    public LevelEnd levelEnd;
    public ItemPhysics itemPhysics;
    public ScoreManager scoreManager;

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
                    // add points to time and cargo
                    // time bonus points for time remaining on timer
                    // item points bonus for number of items * 100
                    scoreManager.timeBonus = Mathf.FloorToInt(levelTimer.GetTimeRemaining());
                    scoreManager.cargoBonus = itemPhysics.itemBoxes.Count * 100f;
                    levelEnd.Win();
                }
            }

            return;
        }
    }
}
