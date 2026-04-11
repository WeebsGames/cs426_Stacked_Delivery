using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    
    public List<AudioClip> clips;
    public AudioSource source;

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying && source.time == 0f)
        {
            source.clip = clips[UnityEngine.Random.Range(0,clips.Count)];
            source.Play();
        }
    }
}
