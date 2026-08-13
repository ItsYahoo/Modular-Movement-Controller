public class IsFallingCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return !context.GroundDetector.isGrounded;
    }
}