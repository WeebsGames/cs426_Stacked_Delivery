using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Shows the level end panel when the player finishes or runs out of 
// time.
public class LevelEnd : MonoBehaviour
{
    public GameObject panel;
    public ScoreManager scoreManager;

    public TMP_Text driftPointsText;
    public TMP_Text timePointsText;
    public TMP_Text itemPointsText;
    public TMP_Text totalPointsText;
    public TMP_Text titleText;

    public GameObject nextLevelButton;
    public List<GameObject> others;
    //public List<AudioSource> muteAudio;
    public int nextLevel;


    // Show the panel with a "Level Complete!" message when the player wins.
    public void Win()
    {
        titleText.text = "Level Complete!";
        titleText.color = Color.green;
        nextLevelButton.SetActive(true);
        PlayerPrefs.SetInt("Level" + nextLevel, 1);

        driftPointsText.gameObject.SetActive(true);
        timePointsText.gameObject.SetActive(true);
        itemPointsText.gameObject.SetActive(true);
        totalPointsText.gameObject.SetActive(true);

        driftPointsText.text = "Drift bonus points: " + Mathf.RoundToInt(scoreManager.driftPoints);
        timePointsText.text = "Time bonus points: " + Mathf.RoundToInt(scoreManager.timeBonus);
        itemPointsText.text = "Items bonus points: " + Mathf.RoundToInt(scoreManager.cargoBonus);
        totalPointsText.text = "Total points: " + Mathf.RoundToInt(scoreManager.GetTotal());

        ShowPanel();
    }

    // Show the panel when the timer runs out.
    public void Lose()
    {
        titleText.text = "Time's up, Game over";
        titleText.color = Color.red;
        nextLevelButton.SetActive(false);
        ShowPanel();
    }

    // Show the panel when the player finished but lost their cargo on the way.
    public void LoseCargo()
    {
        titleText.text = "Cargo Lost, try again...";
        titleText.color = Color.red;
        nextLevelButton.SetActive(false);
        ShowPanel();
    }

    // Pause the game, mute audio, hide HUD, and show the end panel.
    private void ShowPanel()
    {
        Debug.Log("show panel called");
        panel.SetActive(true);

        GameObject car = GameObject.FindWithTag("Player");
        if (car != null)
        {
            AudioSource[] carAudio = car.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource audio in carAudio)
            {
                Debug.Log("car audio being muted: " + audio.name);
                audio.Pause();
                audio.mute = true;
            }
        }

        foreach (GameObject other in others)
        {
            other.SetActive(false);
        }

        Time.timeScale = 0f;
    }

    // Reload the current scene.
    public void Restart()
    {
        //AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Return to the main menu.
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu2");
    }

    // Load the next level scene.
    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("track" + nextLevel);
    }
}