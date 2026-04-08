using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform lvlCamPos;
    public Transform mainCamPos;
    public GameObject mainButtons;
    public GameObject levelButtons;
    public void PlayGame()
    {
        // SceneManager.LoadScene("LevelSelect");
        Camera.main.transform.position = lvlCamPos.position;
        Camera.main.transform.rotation = lvlCamPos.rotation;
        mainButtons.SetActive(false);
        levelButtons.SetActive(true);

    }
    public void Back()
    {
        mainButtons.SetActive(true);
        levelButtons.SetActive(false);
        Camera.main.transform.position = mainCamPos.position;
        Camera.main.transform.rotation = mainCamPos.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}