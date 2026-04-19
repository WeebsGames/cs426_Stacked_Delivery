using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Shows the level end panel when the player finishes or runs out of 
// time.
public class LevelEnd : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text titleText;
    public GameObject nextLevelButton;
    public List<GameObject> others;
    public List<AudioSource> muteAudio;
    public string nextLevelScene;

    // Show the panel with a "Level Complete!" message when the player wins.
    public void Win()
    {
        titleText.text = "Level Complete!";
        titleText.color = Color.green;
        nextLevelButton.SetActive(true);
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
        panel.SetActive(true);

        foreach (AudioSource audio in muteAudio)
        {
            audio.Pause();
            audio.mute = true;
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
        SceneManager.LoadScene(nextLevelScene);
    }
}