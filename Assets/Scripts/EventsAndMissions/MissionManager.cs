using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Transform waypointParent; 
    private List<Waypoint> waypoints = new List<Waypoint>();
    private int currentWaypointIndex = 0;

    [SerializeField] private TextMeshProUGUI uiMessage;
    //[SerializeField] private Transform taskIndicator;
    [SerializeField] private Color textColor = Color.cyan;

    private void OnEnable()
        => Waypoint.OnWaypointReached += HandleWaypointReached;
    private void OnDisable()
        => Waypoint.OnWaypointReached -= HandleWaypointReached;
    
    private void Start()
    {
        /*taskIndicator.TryGetComponent<TextMeshPro>(out TextMeshPro textMeshPro);
        uiMessage = textMeshPro;*/

        LoadWaypoints();
        ActivateCurrentWaypoint();
    }

    private void HandleWaypointReached(string waypointName)
    {
        Debug.Log($"Waypoint '{waypointName}' reached.");

        currentWaypointIndex++;
        ActivateCurrentWaypoint();
    }

    private void LoadWaypoints()
    {
        waypoints.Clear();

        if (waypointParent != null)
        {
            Waypoint[] foundWaypoints = waypointParent.GetComponentsInChildren<Waypoint>();
            waypoints.AddRange(foundWaypoints);
            Debug.Log($"Loaded {waypoints.Count} waypoints from '{waypointParent.name}'.");
        }
        else Debug.LogError("Waypoint Parent is not assigned in the MissionManager.");
        
    }

    private void ActivateCurrentWaypoint()
    {
        foreach (var waypoint in waypoints)
        {
            if (waypoint.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                renderer.enabled = false;
        }

        if (currentWaypointIndex < waypoints.Count)
        {
            Waypoint currentWaypoint = waypoints[currentWaypointIndex];

            if (currentWaypoint.TryGetComponent<MeshRenderer>(out MeshRenderer currentRenderer))
                currentRenderer.enabled = true;

            currentWaypoint.ActivateWaypoint();
            UpdateMessage(currentWaypoint.waypointName);
            Debug.Log($"Activating Waypoint: {currentWaypoint.waypointName}");
        }
        else Debug.Log("All waypoints completed! Mission Complete.");
    }

    private void UpdateMessage(string waypointName)
    {
        string displayText;

        if (waypointName == "New Waypoint")
            displayText = $"Reach <color=#{ColorUtility.ToHtmlStringRGB(textColor)}>Destination</color>";
        else displayText = $"Reach <color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{waypointName}</color>";

        if (uiMessage != null)
            uiMessage.text = displayText;     
    }
}
