
using System.Runtime.InteropServices;

public class IsMovingCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return PlayerInputReader.instance.IsMoving();
    }
}

public class NotMovingCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return !PlayerInputReader.instance.IsMoving();
    }
}

public class IsRunningCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return PlayerInputReader.instance.sprintHeld
               && context.StaminaResource.GetCurrentStamina() > 0f 
               && context.StaminaResource.GetCurrentRegenBuffer() <= 0f;
    }
}

public class NotRunningCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return !PlayerInputReader.instance.sprintHeld;
    }
}

public class IsNotDashStateCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return context.dashStayTimer <= 0f;
    }
}

public class IsDashingCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return PlayerInputReader.instance.playerInput.Player.Dash.triggered && context.dashCooldown <= 0;
    }
}

public class IsJumpingCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return PlayerInputReader.instance.playerInput.Player.Jump.triggered;
    }
}

public class IsNotLandStateCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return context.landStayTimer <= 0f;
    }
}

public class LeftTheGroundCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return context.hasLeftGround && context.verticalVelocity <= 0f;
    }
}

public class HasJumpBufferCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return context.JumpBufferTimer <= context.MovementSettings.GetCoyoteTime()
               && PlayerInputReader.instance.playerInput.Player.Jump.triggered
               && context.MovementStateMachine.previousState is not PlayerJumpState;
    }
}
