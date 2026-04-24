using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public Object skyline;
    public Object trueno;
    public Transform startPos;
    public CameraFollow cameraFollow;

    Object newCar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string loadCar = PlayerPrefs.GetString("Car");

        switch (loadCar)
        {
            case "AE86":
                newCar = Instantiate(trueno, startPos.position, startPos.rotation);
                break;
            case "R32":
                newCar = Instantiate(skyline, startPos.position, startPos.rotation);
                break;
        }

        cameraFollow.FindCar();
    }
}
