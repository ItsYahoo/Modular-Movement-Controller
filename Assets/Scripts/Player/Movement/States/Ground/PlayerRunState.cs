using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerRunState : PlayerMovementStateBase
{
    public PlayerRunState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Run State");
        stateData.SpeedLinesController.SetIntensity(0.25f);
    }

    /*public override PlayerStates ReturnNewState()
    {
        bool stillHasStamina = stateData.StaminaResource.Drain(
            stateData.MovementSettings.GetSprintCost(), 
            Time.deltaTime, true);
        
        if (!stillHasStamina)
            return PlayerStates.Walk;
        
        if (!PlayerInputReader.instance.IsMoving())
            return PlayerStates.Idle;
        
        if (!PlayerInputReader.instance.sprintHeld)
            return PlayerStates.Walk;
        
        if (!stateData.GroundDetector.isGrounded)
            return PlayerStates.Fall;
        
        if (CanDash())
            return PlayerStates.Dash;
        
        if (PlayerInputReader.instance.playerInput.Player.Jump.triggered)
            return PlayerStates.Jump;

        return StateKey;
    }*/

    public override void ExitState()
    {
        stateData.SpeedLinesController.SetIntensity(0f);
    }
}