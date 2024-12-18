using UnityEngine;

public class PatrolingState : EnemyState
{
    private int currentWaypointIndex;
    private float idleChance = 0.1f; // 10% chance to enter IdleState at each waypoint

    public PatrolingState(int waypointIndex)
    {
        currentWaypointIndex = waypointIndex;
    }

    public override void EnterState(StateMachine stateMachine)
    {
        //Debug.Log($"[PatrolingState] Entering state at waypoint {currentWaypointIndex}");
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        if (stateMachine.Waypoints == null || stateMachine.Waypoints.Length == 0)
        {
            //Debug.LogError("[PatrolingState] No waypoints assigned!");
            return;
        }

        // Get the current waypoint
        Transform currentWaypoint = stateMachine.Waypoints[currentWaypointIndex];
        //Debug.Log($"[PatrolingState] Current waypoint: {currentWaypoint.name}");

        // Move toward the current waypoint
        Vector3 direction = (currentWaypoint.position - stateMachine.transform.position).normalized;
        stateMachine.transform.position = Vector3.MoveTowards(
            stateMachine.transform.position,
            new Vector3(currentWaypoint.position.x, stateMachine.transform.position.y, currentWaypoint.position.z),
            stateMachine.PatrolSpeed * Time.deltaTime
        );
        //Debug.Log($"[PatrolingState] Moving to waypoint {currentWaypointIndex}, Distance: {Vector3.Distance(stateMachine.transform.position, currentWaypoint.position)}");

        // Check if the waypoint is reached
        if (Vector3.Distance(stateMachine.transform.position, currentWaypoint.position) < 1.5f)
        {
            // Roll the dice for IdleState
            if (Random.value < idleChance)
            {
                //Debug.Log("[PatrolingState] Random stop triggered. Switching to IdleState.");
                stateMachine.SwitchState(new IdleState(currentWaypointIndex));
                return; // Exit update after switching states
            }

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % stateMachine.Waypoints.Length;
            stateMachine.SetCurrentWaypointIndex(currentWaypointIndex);
            //Debug.Log($"[PatrolingState] Reached waypoint. Moving to next waypoint {currentWaypointIndex}");
        }

        // Transition to ChaseState if the player is detected
        if (stateMachine.IsPlayerDetected())
        {
            //Debug.Log("[PatrolingState] Player detected! Switching to ChaseState.");
            stateMachine.SwitchState(new ChaseState(currentWaypointIndex));
        }
    }

    public override void ExitState(StateMachine stateMachine)
    {
        //Debug.Log($"[PatrolingState] Exiting state at waypoint {currentWaypointIndex}");
    }
}
