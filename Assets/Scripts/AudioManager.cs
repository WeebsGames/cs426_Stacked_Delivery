using UnityEngine;
using UnityEngine.Audio;

//
// AudioManager
//
// Manager for controlling audio volume levels
// Controls master, SFX, and music volume through the GameAudioMixer
//hooked up to UI sliders in a settings menu
//
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //
    // SetMasterVolume()
    //
    // Sets the master volume level from 0-1 value
    //
    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    //
    // SetSFXVolume()
    //
    // Sets the SFX volume level from a 0-1 value
    //
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    //
    // SetMusicVolume()
    //
    // Sets the music volume level from 0-1 value
    //
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
}