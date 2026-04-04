using UnityEngine;

public class RoadsideFlagger : MonoBehaviour
{
    [Header("Scene References")]
    public Animator animator;

    [Header("Animator Parameters")]
    public string carNearParameter = "CarNear";
    public string stopParameter = "StopTraffic";

    [Header("Trigger Rules")]
    public string playerTag = "Player";
    public bool stopTrafficWhenNear = true;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (animator == null)
        {
            return;
        }

        if (!other.CompareTag(playerTag) && !other.transform.root.CompareTag(playerTag))
        {
            return;
        }

        animator.SetBool(carNearParameter, true);
        animator.SetBool(stopParameter, stopTrafficWhenNear);
    }

    void OnTriggerExit(Collider other)
    {
        if (animator == null)
        {
            return;
        }

        if (!other.CompareTag(playerTag) && !other.transform.root.CompareTag(playerTag))
        {
            return;
        }

        animator.SetBool(carNearParameter, false);
        animator.SetBool(stopParameter, false);
    }
}
