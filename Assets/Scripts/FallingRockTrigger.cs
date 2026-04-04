using UnityEngine;

public class FallingRockTrigger : MonoBehaviour
{
    public FallingRockFSM rockFSM;
    public string playerTag = "Player";

    void Reset()
    {
        Collider triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (rockFSM == null)
        {
            return;
        }

        if (!other.CompareTag(playerTag) && !other.transform.root.CompareTag(playerTag))
        {
            return;
        }

        rockFSM.NotifyPlayerEntered();
    }

    void OnTriggerExit(Collider other)
    {
        if (rockFSM == null)
        {
            return;
        }

        if (!other.CompareTag(playerTag) && !other.transform.root.CompareTag(playerTag))
        {
            return;
        }

        rockFSM.NotifyPlayerExited();
    }
}
