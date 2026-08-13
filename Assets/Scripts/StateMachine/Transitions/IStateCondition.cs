public interface IStateCondition<TContext>
{
    bool Evaluate(TContext context);
}