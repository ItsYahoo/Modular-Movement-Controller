using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerRunState : PlayerMovementStateBase
{
    public PlayerRunState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Run State");
    }
    
    public override void TickState()
    {
        base.TickState();
        
        if (!PlayerInputReader.instance.IsMoving())
            stateData.MovementStateMachine.ChangeState(PlayerStates.Idle);
        
        if (!PlayerInputReader.instance.sprintHeld)
            stateData.MovementStateMachine.ChangeState(PlayerStates.Walk);
    }
}