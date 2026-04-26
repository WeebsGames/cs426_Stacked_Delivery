using Unity.VisualScripting;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public Object skyline;
    public Object trueno;
    public Object civic;
    public Transform ae86pos;
    public Transform r32pos;
    public Transform eg6pos;

    Object newCar;

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

    public void EG6()
    {
        PlayerPrefs.SetString("Car", "EG6");
        updateCar();
    }

    public void updateCar()
    {
        GameObject target = GameObject.FindWithTag("Player");
        string loadCar = PlayerPrefs.GetString("Car");
        
        if(target != null)
        {
            Destroy(target);
        }

        switch (loadCar)
        {
            case "AE86":
                newCar = Instantiate(trueno, ae86pos.position, ae86pos.rotation);
                break;
            case "R32":
                newCar = Instantiate(skyline, r32pos.position, ae86pos.rotation);
                break;
            case "EG6":
            print("attempting to spawn EG6");
                newCar = Instantiate(civic, eg6pos.position, eg6pos.rotation);
                break;
        }

        newCar.GetComponent<CarControl>().enabled = false;
        AudioSource[] mutes = newCar.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource sound in mutes)
        {
            if(sound.name != "Engine Source")
            {
                sound.mute = true;
                sound.Stop();
            }
        }
    }

    void Start()
    {
        // print("start called");
        if(PlayerPrefs.GetString("Car") == "")
        {
            PlayerPrefs.SetString("Car", "AE86");
            // print("set car to AE86");
        }
        updateCar();
    }
}
