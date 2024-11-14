using UnityEngine;

public class AttackState : EnemyState
{
    public override void EnterState(StateMachine stateMachine)
    {
        base.EnterState(stateMachine);
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(StateMachine stateMachine) 
    {
        base.ExitState(stateMachine); 
    }
}
