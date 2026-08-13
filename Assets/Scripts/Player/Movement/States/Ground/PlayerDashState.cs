using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerDashState : PlayerMovementStateBase
{
    public PlayerDashState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) { }
    private float dashStayTimer;

    public override void EnterState()
    {
        dashStayTimer = stateData.MovementSettings.GetDashDuration();
        // TODO: Add player boost direction
    }

    public override void TickState()
    {
        base.TickState();
        dashStayTimer -= Time.deltaTime;
    }

    /*public override PlayerStates ReturnNewState()
    {
        if (dashStayTimer > 0f)
            return StateKey;

        if (!stateData.GroundDetector.isGrounded)
            return PlayerStates.Fall;

        if (PlayerInputReader.instance.IsMoving())
            return PlayerInputReader.instance.sprintHeld ? PlayerStates.Run : PlayerStates.Walk;
        
        return PlayerStates.Idle;
    }*/
}