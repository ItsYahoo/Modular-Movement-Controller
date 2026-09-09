using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerDashState : PlayerMovementStateBase
{
    public PlayerDashState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) { }

    public override void EnterState()
    {
        stateData.dashStayTimer = stateData.MovementSettings.GetDashDuration();
        // TODO: Add player boost direction
    }

    public override void TickState()
    {
        base.TickState();
        stateData.dashStayTimer -= Time.deltaTime;
    }

    public override void ExitState()
    {
        stateData.dashStayTimer = 0f;
    }
}