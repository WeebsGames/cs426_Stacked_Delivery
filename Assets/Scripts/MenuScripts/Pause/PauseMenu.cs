using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Transform pausePos;
    public GameObject cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(cam.GetComponent<CameraFollow>().state == CameraFollow.State.PlayState)
            {
                cam.GetComponent<CameraFollow>().state = CameraFollow.State.PauseState;
            } else
            {
                cam.GetComponent<CameraFollow>().state = CameraFollow.State.PlayState;
            }
            
        }
    }
}
