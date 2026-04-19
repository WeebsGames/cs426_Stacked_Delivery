using UnityEngine;
using UnityEngine.AI;

// Drives the deer's Animator blend tree using its NavMeshAgent speed.
// Sets the Vert parameter so the animator blends between Idle, Walk, and Run.
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class DeerAnimationDriver : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 4f;

    private NavMeshAgent agent;
    private Animator animator;

    // Cache the NavMeshAgent and Animator components.
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Convert current speed into a Vert value (0 = idle, 1 = walk, 2 = run).
    private void Update()
    {
        float speed = agent.velocity.magnitude;
        float vert = 0f;

        if (speed > walkSpeed)
        {
            vert = 2f;
        }
        else if (speed > 0.1f)
        {
            vert = 1f;
        }

        animator.SetFloat("Vert", vert);
    }
}