using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StickScript : MonoBehaviour
{
    
    public GameObject cam;
    public Transform resetPos;

    CameraFollow.State targetState = CameraFollow.State.PauseState;

    void OnTriggerEnter(Collider other)
    {
        // print(other.name);
        switch (other.name)
        {
            case "DriveTrigger":
                targetState = CameraFollow.State.PlayState;
                break;
            case "ReverseTrigger":
                targetState = CameraFollow.State.MenuState;
                break;
        }
    }

    void OnMouseUp()
    {
        print("mouse released with targetState: " + targetState);
        if(targetState == CameraFollow.State.MenuState)
        {
            SceneManager.LoadScene("MainMenu2");
        }
        cam.GetComponent<CameraFollow>().state = targetState;
        targetState = CameraFollow.State.PauseState;
        transform.position = resetPos.position;
    }
}
