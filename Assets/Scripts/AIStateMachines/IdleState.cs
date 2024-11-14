using UnityEngine;

public class IdleState : EnemyState
{
    private float idleTimer;
    private int waypointIndex; 

    // Constructor to set the current waypoint index
    public IdleState(int currentWaypointIndex)
    {
        waypointIndex = currentWaypointIndex;
    }

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Entering Idle State");
        idleTimer = Random.Range(2f, 5f); // Random idle duration
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0 && stateMachine.WaypointParent != null)
        {
            Debug.Log("Idle State finished. Resuming PatrolingState.");
            stateMachine.SwitchState(new PatrolingState(waypointIndex)); // Resume patrol
        }
    }

    public override void ExitState(StateMachine stateMachine)
    {
        Debug.Log("Exiting Idle State");
    }
}
