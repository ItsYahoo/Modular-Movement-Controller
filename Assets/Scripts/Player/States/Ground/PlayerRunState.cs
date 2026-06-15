using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerRunState : PlayerMovementStateBase
{
    public PlayerRunState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Run State");
    }

    public override PlayerStates ReturnNewState()
    {
        if (!PlayerInputReader.instance.IsMoving())
            return PlayerStates.Idle;
        
        if (!PlayerInputReader.instance.sprintHeld)
            return PlayerStates.Walk;
        
        if (!stateData.GroundDetector.isGrounded)
            return PlayerStates.Fall;
        
        if (PlayerInputReader.instance.playerInput.Player.Jump.triggered)
            return PlayerStates.Jump;

        return StateKey;
    }
}