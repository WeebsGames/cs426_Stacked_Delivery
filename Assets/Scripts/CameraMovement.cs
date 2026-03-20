using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform car;
    public float smoothing = 5f;
    public float cameraDistancce = 10f;

    Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - car.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCamPos = car.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        // transform.RotateAround(car.transform.up, car.eulerAngles.x);
    }

}
