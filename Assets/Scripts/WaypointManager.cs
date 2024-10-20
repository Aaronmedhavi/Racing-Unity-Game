using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;

    [Header("Waypoints")]
    public Transform[] waypoints;
    public float[] cumulativeDistances;
    public float totalTrackLength;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeWaypoints();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void InitializeWaypoints()
    {
        // Assume waypoints are children of WaypointsParent
        GameObject waypointParent = GameObject.Find("WaypointsParent");
        int numWaypoints = waypointParent.transform.childCount;
        waypoints = new Transform[numWaypoints];

        for (int i = 0; i < numWaypoints; i++)
        {
            waypoints[i] = waypointParent.transform.GetChild(i);
        }
        CalculateCumulativeDistances();
    }
    void OnDrawGizmos()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            // Draw spheres at each waypoint
            Gizmos.color = Color.green;
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint != null)
                {
                    Gizmos.DrawSphere(waypoint.position, 1f);
                }
            }

            // Draw lines between waypoints
            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Transform currentWaypoint = waypoints[i];
                Transform nextWaypoint = waypoints[(i + 1) % waypoints.Length]; // Loop back to the first waypoint
                if (currentWaypoint != null && nextWaypoint != null)
                {
                    Gizmos.DrawLine(currentWaypoint.position, nextWaypoint.position);
                }
            }
        }
    }
    void CalculateCumulativeDistances()
    {
        cumulativeDistances = new float[waypoints.Length];
        float totalDistance = 0f;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i > 0)
            {
                float distance = Vector3.Distance(waypoints[i - 1].position, waypoints[i].position);
                totalDistance += distance;
            }
            cumulativeDistances[i] = totalDistance;
        }

        // Add distance from last waypoint to first to complete the loop
        float lastDistance = Vector3.Distance(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        totalTrackLength = totalDistance + lastDistance;
    }
}
