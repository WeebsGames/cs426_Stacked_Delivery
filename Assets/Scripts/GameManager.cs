using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public Object skyline;
    public Object trueno;
    public Object civic;
    public Transform startPos;
    public CameraFollow cameraFollow;
    public SpeedometerUI speedometerUI;
    public BystanderAnimator bystanderAnimator;
    public StabilityMeterUI stabilityMeterUI;
    public SimplePause simplePause;
    public finishscirpt finishscirpt;

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
            case "EG6":
                newCar = Instantiate(civic, startPos.position, startPos.rotation);
                break;
        }

        if(cameraFollow != null)
        {
            cameraFollow.FindCar();
        }
        if(speedometerUI != null)
        {
            speedometerUI.FindCar();
        }
        if(bystanderAnimator != null)
        {
            bystanderAnimator.FindCar();
        }
        if(stabilityMeterUI != null)
        {
            stabilityMeterUI.FindCar();
        }
        if (finishscirpt != null)
        {
            finishscirpt.FindCar();
        }
        
    }
}
