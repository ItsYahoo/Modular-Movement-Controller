using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerIdleState : PlayerMovementStateBase
{
    public PlayerIdleState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    public override PlayerStates ReturnNewState()
    {
        if (PlayerInputReader.instance.IsMoving())
            return PlayerStates.Walk;
        
        if (!stateData.GroundDetector.isGrounded)
            return PlayerStates.Fall;
        
        if (PlayerInputReader.instance.playerInput.Player.Jump.triggered)
            return PlayerStates.Jump;
        
        return StateKey;
    }
}
