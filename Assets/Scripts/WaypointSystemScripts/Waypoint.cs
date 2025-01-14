using UnityEngine;
using System;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    public string waypointName; // Unique name
    [HideInInspector] public TaskType taskType = TaskType.ReachDestination; // Default task type
    [HideInInspector] public GameObject EnemyParent; // For KillEnemies task
    [HideInInspector] public GameObject TargetObject; // For DestroyObject task
    [HideInInspector] public GameObject CollectableItemsParent;

    public bool IsActive = false; // Tracks if the waypoint is currently active
    public static event Action<string> OnWaypointReached; // Event when waypoint task is completed

    private int _currentKillCount = 0;
    private int _totalItemsToCollet;
    private int _itemsColected;

    public enum TaskType
    {
        ReachDestination,
        KillEnemies,
        DestroyObject,
        CollectItem,
        DefendArea
    }

    private void Awake()
    {
        if (CollectableItemsParent != null)
        {
            _totalItemsToCollet = CollectableItemsParent.transform.childCount;
        }

        _itemsColected = 0;
    }
    private void OnEnable()
    {
        KillableDummy.OnEnemyKilled += HandleEnemyKilled;
        DestroyableObject.OnObjectDestroyed += HandleObjectDestroyed;
        CollectableItem.OnItemCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        KillableDummy.OnEnemyKilled -= HandleEnemyKilled;
        DestroyableObject.OnObjectDestroyed -= HandleObjectDestroyed;
        CollectableItem.OnItemCollected -= HandleItemCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActive && other.CompareTag("Player"))
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
                if (EnemyParent != null)
                {
                    _currentKillCount = 0;
                    CheckKillEnemies();
                }
                break;

            case TaskType.DestroyObject:
                if (TargetObject != null)
                {
                    CheckDestroyObject();
                }
                break;

            case TaskType.CollectItem:
                if (CollectableItemsParent != null)
                {
                    _itemsColected = 0;
                    CheckCollectItems();
                }
                break;

            case TaskType.DefendArea:
                if (EnemyParent != null)
                {
                    
                }
                break;
        }
    }

    private void HandleEnemyKilled(GameObject enemy)
    {
        if (EnemyParent == null || !IsActive) return;

        if (enemy.transform.IsChildOf(EnemyParent.transform))
        {
            _currentKillCount++;

            if (_currentKillCount >= EnemyParent.transform.childCount)
            {
                CompleteTask();
            }
        }
    }

    private void HandleObjectDestroyed(GameObject destroyedObject)
    {
        if (TargetObject == null || !IsActive) return;

        if (destroyedObject == TargetObject)
        {            
            CompleteTask();
        }
    }

    private void CheckKillEnemies()
    {
        if (EnemyParent.transform.childCount == 0)
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
        if (TargetObject == null)
        {
            CompleteTask();
        }
        else
        {
            Debug.Log($"Waypoint {waypointName}: Destroy {TargetObject.name} to complete this task.");
        }
    }

    private void CompleteTask()
    {
        Debug.Log($"Task for waypoint '{waypointName}' completed!");
        IsActive = false;
        OnWaypointReached?.Invoke(waypointName); // Notify MissionManager
    }

    public void ActivateWaypoint()
    {
        IsActive = true;
        Debug.Log($"Waypoint '{waypointName}' is now active.");
    }

    private void HandleItemCollected(CollectableItem collectedItem)
    {
        if (CollectableItemsParent == null || !IsActive) return; 
        
        if (collectedItem.transform.IsChildOf(CollectableItemsParent.transform))
        {
            _itemsColected++;
            
            if (_itemsColected >= _totalItemsToCollet)
            {
                CompleteTask();
            }
        }
    }

    private void CheckCollectItems()
    {
        if (CollectableItemsParent.transform.childCount == 0)
        {
            CompleteTask();
        }
    }
}
