using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlOrbScript : MonoBehaviour
{
    public int targetLvl;

    void OnMouseDown()
    {
        SceneManager.LoadScene("Track" + targetLvl);
    }
}
