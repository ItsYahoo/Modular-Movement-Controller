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

    public override void TickState()
    {
        base.TickState();
        stateData.StaminaResource.Drain(stateData.MovementSettings.GetSprintCost(), Time.deltaTime, true);
    }

    public override void ExitState()
    {
        stateData.SpeedLinesController.SetIntensity(0f);
    }
}