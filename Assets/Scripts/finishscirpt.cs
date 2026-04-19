using UnityEngine;

public class finishscirpt : MonoBehaviour
{

    public AudioSource finishSound;
    public AudioClip finishClip;
    public LevelTimer levelTimer;

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

            return;
        }
    }
}
