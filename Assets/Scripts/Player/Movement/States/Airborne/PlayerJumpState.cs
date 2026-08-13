using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerJumpState : PlayerMovementStateBase
{
    public PlayerJumpState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}
    private bool hasLeftGround;
    private float jumpTimer;
    private bool hasLaunched;

    public override void EnterState()
    {
        Debug.Log("Entering Jump State");
        hasLeftGround = false;
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
    }

    /*public override PlayerStates ReturnNewState()
    {
        if (hasLaunched && !stateData.GroundDetector.isGrounded)
            hasLeftGround = true;

        if (hasLeftGround && stateData.verticalVelocity <= 0f)
        {
            return PlayerStates.Fall;
        }

        if (hasLeftGround && stateData.GroundDetector.isGrounded)
        {
            return PlayerStates.Land;
        }

        return StateKey;
    }*/
    
    public override void ExitState()
    {
        stateData.Animator.ResetTrigger("Jump");
    }
}