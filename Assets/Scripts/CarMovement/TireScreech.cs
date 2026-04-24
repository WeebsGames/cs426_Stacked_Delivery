using Unity.Mathematics;
using UnityEngine;

public class TireScreech : MonoBehaviour
{
    
    public WheelCollider wheel;
    public AudioSource source;

    WheelHit hit;

    void Start()
    {
        source.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(wheel.GetGroundHit(out hit))
        {
            // print(hit.sidewaysSlip);
            if(math.abs(hit.sidewaysSlip) > 0.2)
            {
                source.UnPause();
                return;
            }
        }
        source.Pause();
    }
}
