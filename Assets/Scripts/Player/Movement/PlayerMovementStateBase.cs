using UnityEngine;

public abstract class PlayerMovementStateBase : StateBase<PlayerMovementStateMachine.PlayerStates>
{
    protected PlayerMovementStateBase(PlayerMovementStateMachine.PlayerStates stateKey) : base(stateKey)
    {
        // TODO: Add State Data
    }
}
