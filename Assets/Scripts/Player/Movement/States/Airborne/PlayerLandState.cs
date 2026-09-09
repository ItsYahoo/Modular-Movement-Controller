using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerLandState : PlayerMovementStateBase
{
    public PlayerLandState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Land State");
        
        stateData.ImpulseSource.GenerateImpulseWithVelocity(new Vector3(0f, -0.25f, 0f)); // Camera Shake
        stateData.ignoreGroundStickForce = false;
        stateData.Animator.SetTrigger("Land");
        stateData.landStayTimer = stateData.MovementSettings.GetLandDuration();
    }

    public override void TickState()
    {
        base.TickState();
        stateData.landStayTimer -= Time.deltaTime;
    }

    public override void ExitState()
    {
        stateData.Animator.ResetTrigger("Land");
    }
}