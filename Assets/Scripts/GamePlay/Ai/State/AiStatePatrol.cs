using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStatePatrol : AiState
{
    [Space(10)]
    [HideInInspector]
    // Specified path
    public Pathway path;

    [HideInInspector]
    // Current destination
    public Waypoint destination;
    public override void Awake()
    {
        base.Awake();
        Debug.Assert(aiBehavior.navAgent, "Wrong initial parameters");
    }
    public override void OnStateEnter(AiState previousState, AiState newState)
    {
        if (path == null)
        {
            // If I have no path - try to find it
            path = FindObjectOfType<Pathway>();
            Debug.Assert(path, "Have no path");
        }
        if (destination == null)
        {
            // Get next waypoint from my path
            destination = path.GetNearestWaypoint(transform.position);
        }
        // Set destination for navigation agent
        aiBehavior.navAgent.destination = destination.transform.position;
        // Start moving
        aiBehavior.navAgent.move = true;
        aiBehavior.navAgent.turn = true;
        // If unit has animator
    }
    public override void OnStateExit(AiState previousState, AiState newState)
    {
        // Stop moving
        aiBehavior.navAgent.move = false;
        aiBehavior.navAgent.turn = false;
    }
    void FixedUpdate()
    {
        if (destination != null)
        {
            // If destination reached
            if ((Vector2)destination.transform.position == (Vector2)transform.position)
            {
                // Get next waypoint from my path
                destination = path.GetNextWaypoint(destination, false);
                if (destination != null)
                {
                    // Set destination for navigation agent
                    aiBehavior.navAgent.destination = destination.transform.position;
                }
            }
        }
    }
    public float GetRemainingPath()
    {
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + path.GetPathDistance(destination));
    }
    public void UpdateDestination(bool getNearestWaypoint)
    {
        if (getNearestWaypoint == true)
        {
            // Get next waypoint from my path
            destination = path.GetNearestWaypoint(transform.position);
        }
        if (enabled == true)
        {
            // Set destination for navigation agent
            aiBehavior.navAgent.destination = destination.transform.position;
        }
    }
    // Update is called once per frame
}
