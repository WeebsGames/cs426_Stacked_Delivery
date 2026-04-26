using UnityEngine;

public class AE86Engine : MonoBehaviour
{
    public AudioSource source;
    public Rigidbody rb;
    // public AudioClip idle;
    // public AudioClip running;
    float idleStart = 54f;
    float idleEnd = 64f;
    bool idling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // source.timeSamples = (int)(loopStart * source.clip.frequency);
        // Resources.Load("Assets/Audio/Initial D Engine SFX  Initial D Collectors Disc.mp3");
        // Resources.Load("Assets/PROMETEO - Car Controller/Sounds/CarEngine.wav");
        // source.clip = running;
        source.Play();
        source.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        source.pitch = 0.5f + rb.linearVelocity.magnitude/12f;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // source.mute = !source.mute;
        }

        // // source.volume = rb.linearVelocity.magnitude;
        // if(rb.linearVelocity.magnitude == 0)
        // {
        //     // idling = true;
        //     // source.volume = 1f;
        //     source.clip = idle;
        // }
        // else
        // {
        //     // idling = false;
        //     source.clip = running;
        // }
    }

    // void idleSwitch()
    // {
    //     source.Stop();
    //     source.clip = idle;
    //     source.volume = 1f;
    //     source.Play();
    //     // Reset to loop start if the current time exceeds the loop end
    //     if (source.time >= idleEnd)
    //     {
    //         source.timeSamples = (int)(idleEnd * source.clip.frequency);
    //     }
    // }
}
