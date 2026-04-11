using UnityEngine;

public class finishscirpt : MonoBehaviour
{
    
    public AudioSource finishSound;
    public AudioClip finishClip;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("finish");
            finishSound.Stop();
            finishSound.clip = finishClip;
            finishSound.Play();
            
            return;
        }
    }
}
