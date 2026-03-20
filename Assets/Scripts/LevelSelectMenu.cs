using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    public void LoadLevel1()
    {
        Debug.Log("Level 1 clicked");
    }

    public void LoadLevel2()
    {
        Debug.Log("Level 2 clicked");
    }

    public void LoadLevel3()
    {
        Debug.Log("Level 3 clicked");
    }

    public void LoadLevel4()
    {
        Debug.Log("Level 4 clicked");
    }

    public void LoadLevel5()
    {
        Debug.Log("Level 5 clicked");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}