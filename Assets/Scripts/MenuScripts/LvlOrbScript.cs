using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlOrbScript : MonoBehaviour
{
    public int targetLvl;
    public bool locked;
    public Material unlockedMat;
    public Material lockedMat;
    public Material selected;

    Renderer render;

    void Start()
    {
        render = GetComponent<Renderer>();
        if(PlayerPrefs.GetInt("Level"+targetLvl) == 1)
        {
            locked = false;
        }
        CheckMat();
    }

    void OnMouseOver()
    {
        if(locked == false)
        {
            render.material = selected;
        }
    }

    void OnMouseExit()
    {
        CheckMat();
    }

    void OnMouseDown()
    {
        print("Track"+targetLvl);
        SceneManager.LoadScene("Track" + targetLvl);
    }

    void CheckMat()
    {
        if (locked)
        {
            render.material = lockedMat;
        } else
        {
            render.material = unlockedMat;
        }
    }
}
