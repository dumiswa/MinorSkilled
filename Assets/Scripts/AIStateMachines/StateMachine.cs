using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StateMachine : MonoBehaviour
{
    private EnemyState currentState;

    [Header("Patrolling Settings")]
    public Transform WaypointParent;
    [HideInInspector] public Transform[] Waypoints;
    public float PatrolSpeed = 2f;

    [Header("Chase Settings")]
    public float ChaseSpeed = 5f;
    public float DetectionRange = 5f;
    public float LoseSightRange = 3f;
    [HideInInspector] public Transform Player;

    private int currentWaypointIndex = 0; 

    private void Start()
    {
        // Find the player by tag
        Player = GameObject.FindWithTag("Player")?.transform;

        if (WaypointParent != null)
        {
            int waypointCount = WaypointParent.transform.childCount;
            Waypoints = new Transform[waypointCount];
            for (int i = 0; i < waypointCount; i++)
            {
                Waypoints[i] = WaypointParent.transform.GetChild(i);
            }
        }
        else Debug.LogError("Waypoint Parent is not assigned!");


        // Initialize the first state
        // ~ PatrollingState if there are waypoints assigned
        // ~ IdleState if there are no waypoitns assigned
        if (WaypointParent != null)
        {
            currentState = new PatrolingState(currentWaypointIndex);
            currentState.EnterState(this);
        }
        else currentState = new IdleState(currentWaypointIndex);
        
    }

    private void Update() => currentState?.UpdateState(this);
    

    public void SwitchState(EnemyState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public bool IsPlayerDetected()
    {
        if (Player == null) return false;
        return Vector3.Distance(transform.position, Player.position) <= DetectionRange;
    }

    public bool HasLostPlayer()
    {
        if (Player == null) return true;
        return Vector3.Distance(transform.position, Player.position) > LoseSightRange;
    }

    public int GetCurrentWaypointIndex()
    {
        return currentWaypointIndex;
    }

    public void SetCurrentWaypointIndex(int index)
    {
        currentWaypointIndex = index;
    }
}

