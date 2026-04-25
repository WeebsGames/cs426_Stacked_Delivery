using Unity.VisualScripting;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public Object skyline;
    public Object trueno;
    public Transform ae86pos;
    public Transform r32pos;

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
        }

        newCar.GetComponent<CarControl>().enabled = false;
        AudioSource[] mutes = newCar.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource sound in mutes)
        {
            if(sound.name != "Engine Source")
            {
                sound.mute = true;
            }
        }
    }

    void Start()
    {
        if(PlayerPrefs.GetString("Car") == null)
        {
            PlayerPrefs.SetString("Car", "AE86");
        }
        updateCar();
    }
}
