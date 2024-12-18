using UnityEngine;
using System;

public class Waypoint : MonoBehaviour
{
    public string waypointName; // Unique name
    [HideInInspector] public TaskType taskType = TaskType.ReachDestination; // Default task type
    [HideInInspector] public GameObject enemyParent; // For KillEnemies task
    [HideInInspector] public GameObject targetObject; // For DestroyObject task

    public bool isActive = false; // Tracks if the waypoint is currently active
    public static event Action<string> OnWaypointReached; // Event when waypoint task is completed

    //private int currentKillCount = 0;

    public enum TaskType
    {
        ReachDestination,
        KillEnemies,
        DestroyObject
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Debug.Log($"Waypoint {waypointName} triggered.");
            HandleTask();
        }
    }

    private void HandleTask()
    {
        switch (taskType)
        {
            case TaskType.ReachDestination:
                CompleteTask();
                break;

            case TaskType.KillEnemies:
                if (enemyParent != null)
                {
                    //currentKillCount = 0;
                    CheckKillEnemies();
                }
                break;

            case TaskType.DestroyObject:
                if (targetObject != null)
                {
                    CheckDestroyObject();
                }
                break;
        }
    }

    private void CheckKillEnemies()
    {
        if (enemyParent.transform.childCount == 0)
        {
            CompleteTask();
        }
        else
        {
            Debug.Log($"Waypoint {waypointName}: Kill all enemies to complete this task.");
        }
    }

    private void CheckDestroyObject()
    {
        if (targetObject == null)
        {
            CompleteTask();
        }
        else
        {
            Debug.Log($"Waypoint {waypointName}: Destroy {targetObject.name} to complete this task.");
        }
    }

    private void CompleteTask()
    {
        Debug.Log($"Task for waypoint '{waypointName}' completed!");
        isActive = false;
        OnWaypointReached?.Invoke(waypointName); // Notify MissionManager
    }

    public void ActivateWaypoint()
    {
        isActive = true;
        Debug.Log($"Waypoint '{waypointName}' is now active.");
    }
}
