using UnityEngine;
using UnityEngine.AI;

//
// Moves the deer between a set of waypoints using NavMesh pathfinding.
//
public class DeerNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    [SerializeField] private Transform[] waypoints;
    
    //
    // Initializes the NavMesh agent and moves to the first waypoint.
    //
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    //
    // Checks if the agent has reached the current waypoint and moves to the next.
    //
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !agent.isStopped)
        {
            agent.isStopped = true;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            MoveToNextWaypoint();
        }
    }

    //
    // Sets the agent's destination to the current waypoint.
    //
    void MoveToNextWaypoint()
    {
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }
}
