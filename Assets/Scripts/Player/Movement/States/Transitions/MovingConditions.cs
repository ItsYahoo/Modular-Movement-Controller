
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
        return PlayerInputReader.instance.sprintHeld;
    }
}

public class NotRunningCondition : IStateCondition<PlayerStateData>
{
    public bool Evaluate(PlayerStateData context)
    {
        return !PlayerInputReader.instance.sprintHeld;
    }
}