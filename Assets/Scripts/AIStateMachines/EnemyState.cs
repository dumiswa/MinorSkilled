using UnityEngine;

public abstract class EnemyState
{
    public virtual void EnterState(StateMachine stateMachine) { }

    public abstract void UpdateState(StateMachine stateMachine);

    public virtual void ExitState(StateMachine stateMachine) { }
}

