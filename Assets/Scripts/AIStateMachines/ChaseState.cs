using UnityEngine;

public class ChaseState : EnemyState
{
    private int lastWaypointIndex;

    public ChaseState(int waypointIndex)
    {
        lastWaypointIndex = waypointIndex;
    }

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Entering Chase State");
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        Transform player = stateMachine.Player;

        if (player == null) return;

        // Chase the player
        Vector3 direction = (player.position - stateMachine.transform.position).normalized;
        stateMachine.transform.position += direction * stateMachine.ChaseSpeed * Time.deltaTime;

        // Check if the player is out of range
        if (stateMachine.HasLostPlayer())
        {
            Debug.Log("Player lost. Returning to patrol.");
            stateMachine.SwitchState(new PatrolingState(lastWaypointIndex)); // Resume patrol at the last visited waypoint
        }
    }

    public override void ExitState(StateMachine stateMachine)
    {
        Debug.Log("Exiting Chase State");
    }
}





