using UnityEngine;
using static PlayerMovementStateMachine;

public abstract class PlayerMovementStateBase : StateBase<PlayerStates>
{
    protected readonly PlayerStateData stateData;
    protected PlayerMovementStateBase(PlayerStateData stateData, PlayerStates stateKey) 
        : base(stateKey)
    {
        this.stateData = stateData;
    }

    public override void TickState()
    {
        Move(PlayerInputReader.instance.moveInput);
        stateData.StaminaResource.TickStamina(Time.deltaTime);
    }

    #region Movement and Rotation Logic

     private void Move(Vector2 inputDirection)
    {
        // Apply gravity
        CalculateGravity();

        // Calculate Horizontal Velocity
        Vector3 cameraRelativeDirection = GetCameraRelativeInputDirection(inputDirection);
        CalculateCurrentSpeed();
        CalculateCurrentHorizontalVelocity(cameraRelativeDirection);
        CalculateRotation(cameraRelativeDirection);
        
        // Apply Final Velocity
        Vector3 finalVelocity = new  Vector3(stateData.currentHorizontalVelocity.x, stateData.currentVelocity.y, stateData.currentHorizontalVelocity.z);
        stateData.currentVelocity = finalVelocity;

        stateData.CharacterController.Move(stateData.currentVelocity * Time.deltaTime);
    }

    private void CalculateGravity()
    {
        Vector3 tempYVelocity = stateData.currentVelocity;

        if (stateData.GroundDetector.isGrounded && !stateData.ignoreGroundStickForce)
        {
            stateData.verticalVelocity = 0f;
            tempYVelocity.y = -stateData.MovementSettings.GetGroundStickForce();
        }
        else
        {
            stateData.verticalVelocity += stateData.MovementSettings.GetGravity() * Time.deltaTime;
            tempYVelocity.y = stateData.verticalVelocity;
        }

        stateData.currentVelocity = tempYVelocity;
    }

    private void CalculateCurrentHorizontalVelocity(Vector3 cameraRelativeDirection)
    {
        Vector3 targetHorizontalVelocity = cameraRelativeDirection * stateData.currentSpeed;
        bool hasCurrentVelocity = stateData.currentHorizontalVelocity.sqrMagnitude > 0.01f;

        bool changingDirection = false;
        
        if (PlayerInputReader.instance.IsMoving() && hasCurrentVelocity)
        {
            // Check if the player is trying to change direction by comparing the current
            // velocity direction with the camera relative input direction
            Vector3 currentDirection = stateData.currentHorizontalVelocity.normalized;
            float directionDot = Vector3.Dot(currentDirection, cameraRelativeDirection);

            changingDirection = directionDot < 0.75f;
        }
        
        float speedChangeRate = GetSpeedChangeRate(changingDirection);
        
        stateData.currentHorizontalVelocity = Vector3.MoveTowards(
            stateData.currentHorizontalVelocity, targetHorizontalVelocity, speedChangeRate * Time.deltaTime);
    }

    private void CalculateCurrentSpeed()
    {
        float landingMul = stateData.MovementStateMachine.currentState is PlayerLandState ? 0 : 1;
        float run = stateData.MovementStateMachine.currentState is PlayerRunState ? 1 : 0; // 1 if run, 0 if walk
        float moveSpeed = (run * stateData.MovementSettings.GetRunSpeed() + (1 - run) * 
                            stateData.MovementSettings.GetWalkSpeed()) *
                          (stateData.MovementSettings.GetEnvironmentMultiplier() * landingMul);
        float targetSpeed = PlayerInputReader.instance.IsMoving() ? moveSpeed : 0f;
        
        float speedChangeRate = stateData.currentSpeed < targetSpeed
            ? stateData.MovementSettings.GetAcceleration() : stateData.MovementSettings.GetDeceleration();

        stateData.currentSpeed = Mathf.MoveTowards(
            stateData.currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
        
        stateData.Animator.SetFloat("CurrentSpeed", stateData.currentSpeed);
    }

    private Vector3 GetCameraRelativeInputDirection(Vector2 inputDirection)
    {
        // Get the camera's forward and right vectors
        Vector3 cameraForward = stateData.MainCamera.transform.forward;
        Vector3 cameraRight = stateData.MainCamera.transform.right;
        
        // Project the camera's forward and right vectors onto the horizontal plane
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // Calculate the movement direction relative to the camera
        return (cameraForward * inputDirection.y + cameraRight * inputDirection.x).normalized;
    }

    private void CalculateRotation(Vector3 cameraRelativeDirection)
    {
        // Don't rotate if there is no input
        if (!PlayerInputReader.instance.IsMoving() || stateData.MovementStateMachine.currentState is PlayerLandState)
            return;

        // Don't rotate if the direction is too small
        if (cameraRelativeDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeDirection);

        stateData.PlayerTransform.rotation = Quaternion.Slerp(
            stateData.PlayerTransform.rotation, targetRotation, stateData.MovementSettings.GetTurnSmoothTime() * Time.deltaTime);
    }

    private float GetSpeedChangeRate(bool changingDirection)
    {
        bool isAirborne = !stateData.GroundDetector.isGrounded;

        if (!stateData.GroundDetector.isGrounded)
            return GetAirControlRate();
        
        if (!PlayerInputReader.instance.IsMoving())
            return stateData.MovementSettings.GetDeceleration();
        
        if (changingDirection)
            return stateData.MovementSettings.GetDirectionChangeAcceleration();
        
        return stateData.MovementSettings.GetAcceleration();
    }
    
    private float GetAirControlRate()
    {
        return stateData.MovementSettings.GetAcceleration() 
               * stateData.MovementSettings.GetAirControl();
    }

    #endregion
    
    protected void PreformJump()
    {
        float gravity = stateData.MovementSettings.GetGravity();
        float jumpForce = stateData.MovementSettings.GetJumpForce();
        
        stateData.verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    protected void PreformDash()
    {
        
    }

    protected bool CanDash()
    {
        return PlayerInputReader.instance.playerInput.Player.Dash.triggered 
               && stateData.dashCooldown <= 0;
    }
}
