using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Diagnostics;

public class SimplePause : MonoBehaviour
{
    
    public int level;
    public TMP_Text text;
    public List<GameObject> others; 
    public GameObject pause;
    public List<AudioSource> muteAudio;
    public LevelTimer levelTimer;

    bool paused = false;
    
    public void FindCar()
    {
        GameObject[] muteTag = GameObject.FindGameObjectsWithTag("MuteNoise");
        foreach (GameObject tagged in muteTag)
        {
            muteAudio.Add(tagged.GetComponent<AudioSource>());
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "Level " + level;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
            // print("paused: " + paused);
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Restart();
        }
        
    }

    public void Quit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu2");
    }

    public void PauseToggle()
    {
        paused = !paused;
        if (paused)
        {
            pause.SetActive(paused);
            levelTimer.StopTimer();
            foreach (AudioSource audio in muteAudio)
            {
                audio.Pause();
                audio.mute = paused;
            }
            foreach (GameObject other in others)
            {
                other.SetActive(false);
            }
            Time.timeScale = 0.0f;
            return;
        }
        pause.SetActive(paused);
        levelTimer.StartTimer();
        foreach (AudioSource audio in muteAudio)
        {
            audio.mute = paused;
            audio.UnPause();
        }
        foreach (GameObject other in others)
            {
                other.SetActive(true);
            }
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
