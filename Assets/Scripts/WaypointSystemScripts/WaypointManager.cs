using UnityEngine;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
    public List<Waypoint> waypoints = new List<Waypoint>();

    private void Start()
    {
        foreach (var waypoint in waypoints)
        {
            waypoint.isActive = false; // Disable all waypoints initially
        }

        if (waypoints.Count > 0)
        {
            waypoints[0].isActive = true; // Activate the first waypoint
        }

        Waypoint.OnWaypointReached += OnWaypointReached;
    }

    private void OnWaypointReached(string waypointName)
    {
        Debug.Log($"Waypoint {waypointName} reached in WaypointManager.");
    }

    private void OnDestroy()
    {
        Waypoint.OnWaypointReached -= OnWaypointReached;
    }
}
