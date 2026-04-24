using Unity.VisualScripting;
using UnityEngine;

public class Garage : MonoBehaviour
{

    public void R32()
    {
        PlayerPrefs.SetString("Car", "R32");
        updateCar();
    }

    public void AE86()
    {
        PlayerPrefs.SetString("Car", "AE86");
        updateCar();
    }

    public void updateCar()
    {
        
    }

    void Start()
    {
        if(PlayerPrefs.GetString("Car") == null)
        {
            PlayerPrefs.SetString("Car", "AE86");
        }
    }
}
