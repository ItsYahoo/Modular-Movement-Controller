public class IsFallingCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return !context.GroundDetector.isGrounded;
    }
}

public class IsGroundedCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return context.GroundDetector.isGrounded;
    }
}