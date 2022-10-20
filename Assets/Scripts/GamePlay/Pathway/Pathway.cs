using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pathway : MonoBehaviour
{
    void Update()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        if (waypoints.Length > 1)
        {
            int idx;
            for (idx = 1; idx < waypoints.Length; idx++)
            {
                // Draw blue lines along pathway in edit mode
                Debug.DrawLine(waypoints[idx - 1].transform.position, waypoints[idx].transform.position, new Color(0f, 0f, 0f));
            }
        }
    }

    public Waypoint GetNearestWaypoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;
        foreach (Waypoint waypoint in GetComponentsInChildren<Waypoint>())
        {
            // Calculate distance to waypoint
            Vector3 vect = position - waypoint.transform.position;
            float distance = vect.magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWaypoint = waypoint;
            }
        }
        for (; ; )
        {
            float waypointPathDistance = GetPathDistance(nearestWaypoint);
            Waypoint nextWaypoint = GetNextWaypoint(nearestWaypoint, false);
            if (nextWaypoint != null)
            {
                float myPathDistance = GetPathDistance(nextWaypoint) + (nextWaypoint.transform.position - position).magnitude;
                if (waypointPathDistance <= myPathDistance)
                {
                    break;
                }
                else
                {
                    nearestWaypoint = nextWaypoint;
                }
            }
            else
            {
                break;
            }
        }
        return nearestWaypoint;
    }

    public float GetPathDistance(Waypoint fromWaypoint)
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();
        bool hitted = false;
        float pathDistance = 0f;
        int idx;
        // Calculate remaining path
        for (idx = 0; idx < waypoints.Length; ++idx)
        {
            if (hitted == true)
            {
                // Add distance between waypoint to result
                Vector2 distance = waypoints[idx].transform.position - waypoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
            }
            if (waypoints[idx] == fromWaypoint)
            {
                hitted = true;
            }
        }
        return pathDistance;
    }

    public Waypoint GetPreviousWaypoint(Waypoint currentWaypoint, bool loop)
    {
        Waypoint res = null;
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx > 0)
        {
            idx -= 1;
        }
        else
        {
            idx = transform.childCount - 1;
        }
        if (!(loop == false && idx == transform.childCount - 1))
        {
            res = transform.GetChild(idx).GetComponent<Waypoint>();
        }
        return res;
    }

    public Waypoint GetNextWaypoint(Waypoint currentWaypoint, bool loop)
    {
        Waypoint res = null;
        int idx = currentWaypoint.transform.GetSiblingIndex();
        if (idx < (transform.childCount - 1))
        {
            idx += 1;
        }
        else
        {
            idx = 0;
        }
        if (!(loop == false && idx == 0))
        {
            res = transform.GetChild(idx).GetComponent<Waypoint>();
        }
        return res;
    }
}
