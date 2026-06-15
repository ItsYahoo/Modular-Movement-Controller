using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerLandState : PlayerMovementStateBase
{
    public PlayerLandState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Land State");
        
        stateData.ImpulseSource.GenerateImpulseWithVelocity(new Vector3(0f, -0.25f, 0f));
        stateData.ignoreGroundStickForce = false;
        stateData.Animator.SetTrigger("Land");
    }

    public override PlayerStates ReturnNewState()
    {
        if (PlayerInputReader.instance.IsMoving())
        {
            return PlayerInputReader.instance.sprintHeld ? PlayerStates.Run : PlayerStates.Walk;
        }
        
        return PlayerStates.Idle;
    }
}