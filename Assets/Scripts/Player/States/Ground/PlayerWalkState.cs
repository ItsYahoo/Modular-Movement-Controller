using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerWalkState : PlayerMovementStateBase
{
    public PlayerWalkState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Walk State");
    }


    public override PlayerStates ReturnNewState()
    {
        if (!PlayerInputReader.instance.IsMoving())
            return PlayerStates.Idle;

        if (PlayerInputReader.instance.sprintHeld)
            return PlayerStates.Run;
        
        if (!stateData.GroundDetector.isGrounded)
            return PlayerStates.Fall;
        
        if (PlayerInputReader.instance.playerInput.Player.Jump.triggered)
            return PlayerStates.Jump;

        return StateKey;
    }
}