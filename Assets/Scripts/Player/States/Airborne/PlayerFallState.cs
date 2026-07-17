using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerFallState : PlayerMovementStateBase
{
    public PlayerFallState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}
    private float bufferTimer;

    public override void EnterState()
    {
        Debug.Log("Entering Fall State");
        stateData.Animator.SetTrigger("Fall");

        bufferTimer = 0f;
    }

    public override void TickState()
    {
        base.TickState();
        bufferTimer += Time.deltaTime;
    }

    public override PlayerStates ReturnNewState()
    {
        if (bufferTimer <= stateData.MovementSettings.GetCoyoteTime()
            && PlayerInputReader.instance.playerInput.Player.Jump.triggered
            && stateData.MovementStateMachine.previousState is not PlayerJumpState)
            return PlayerStates.Jump;
        
        if (stateData.GroundDetector.isGrounded)
            return PlayerStates.Land;

        return StateKey;
    }
    
    public override void ExitState()
    {
        stateData.Animator.ResetTrigger("Fall");
    }
}