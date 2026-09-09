using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerJumpState : PlayerMovementStateBase
{
    public PlayerJumpState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}
    private float jumpTimer;
    private bool hasLaunched;

    public override void EnterState()
    {
        Debug.Log("Entering Jump State");
        stateData.hasLeftGround = false;
        jumpTimer = 0f;
        hasLaunched = false;

        stateData.ignoreGroundStickForce = true;
        stateData.Animator.SetTrigger("Jump");
    }

    public override void TickState()
    {
        base.TickState();
        jumpTimer += Time.deltaTime;

        if (!hasLaunched && jumpTimer >= stateData.MovementSettings.GetLaunchDelay())
        {
            PreformJump();
            hasLaunched = true;
        }
        
        if (hasLaunched && !stateData.GroundDetector.isGrounded)
            stateData.hasLeftGround = true;
    }
    
    public override void ExitState()
    {
        stateData.Animator.ResetTrigger("Jump");
    }
}