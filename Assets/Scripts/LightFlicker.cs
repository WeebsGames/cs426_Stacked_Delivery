using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float speed = 1f;
    private Light myLight;
    private float timer;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= speed)
        {
            myLight.enabled = !myLight.enabled;
            timer = 0;
        }
    }
}