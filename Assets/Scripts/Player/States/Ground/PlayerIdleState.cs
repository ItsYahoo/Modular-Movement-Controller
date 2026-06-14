using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerIdleState : PlayerMovementStateBase
{
    public PlayerIdleState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    public override void TickState()
    {
        base.TickState();
        
        if (PlayerInputReader.instance.IsMoving())
            stateData.MovementStateMachine.ChangeState(PlayerStates.Walk);
    }
}
