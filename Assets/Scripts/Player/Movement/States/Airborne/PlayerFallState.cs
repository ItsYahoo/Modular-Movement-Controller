using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerFallState : PlayerMovementStateBase
{
    public PlayerFallState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Fall State");
        stateData.Animator.SetTrigger("Fall");

        stateData.JumpBufferTimer = 0f;
    }

    public override void TickState()
    {
        base.TickState();
        stateData.JumpBufferTimer += Time.deltaTime;
    }
    
    public override void ExitState()
    {
        stateData.Animator.ResetTrigger("Fall");
    }
}