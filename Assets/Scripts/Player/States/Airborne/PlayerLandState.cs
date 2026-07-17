using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerLandState : PlayerMovementStateBase
{
    public PlayerLandState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}
    float landTimer;

    public override void EnterState()
    {
        Debug.Log("Entering Land State");
        
        stateData.ImpulseSource.GenerateImpulseWithVelocity(new Vector3(0f, -0.25f, 0f)); // Camera Shake
        stateData.ignoreGroundStickForce = false;
        stateData.Animator.SetTrigger("Land");
        landTimer = 0f;
    }

    public override void TickState()
    {
        base.TickState();
        
        landTimer += Time.deltaTime;
    }

    public override PlayerStates ReturnNewState()
    {
        if (landTimer < stateData.MovementSettings.GetLandDuration())
            return StateKey;
            
        if (PlayerInputReader.instance.IsMoving())
            return PlayerInputReader.instance.sprintHeld ? PlayerStates.Run : PlayerStates.Walk;
        
        return PlayerStates.Idle;
    }

    public override void ExitState()
    {
        stateData.Animator.ResetTrigger("Land");
    }
}