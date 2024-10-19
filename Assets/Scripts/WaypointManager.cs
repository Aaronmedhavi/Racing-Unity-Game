using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;

    [Header("Waypoints")]
    public Transform[] waypoints;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Automatically find and assign waypoints
            GameObject waypointParent = GameObject.Find("WaypointsParent");
            waypoints = new Transform[waypointParent.transform.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = waypointParent.transform.GetChild(i);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }



}
