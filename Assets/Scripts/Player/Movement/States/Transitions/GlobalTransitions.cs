public class NoStamina : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return context.StaminaResource.GetCurrentStamina() <= 0f;
    }
}