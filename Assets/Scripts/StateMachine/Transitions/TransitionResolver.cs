using System;
using System.Collections.Generic;

public class TransitionResolver<TState, TContext> where TState : Enum
{
    private readonly TransitionRegistry<TState, TContext> registry;
    
    public TransitionResolver(TransitionRegistry<TState, TContext> registry)
    {
        this.registry = registry;
    }
    
    public bool TryResolve(TState currentState, TContext context, out StateTransition<TState, TContext> transition)
    {
        var globalTransitionList = registry.GetGlobalTransitions();
        var localTransitionList = registry.GetLocalTransitions(currentState);
        var groupTransitionList = registry.GetGroupTransitions(currentState);
        StateTransition<TState, TContext> currentTransition = null;
        transition = null;
        
        foreach (var globalTransition in globalTransitionList)
        {
            if (globalTransition.CanTransition(context))
                currentTransition = CheckPriority(currentTransition, globalTransition);
        }

        foreach (var groupTransition in groupTransitionList)
        {
            if (groupTransition.CanTransition(context))
                currentTransition = CheckPriority(currentTransition, groupTransition);
        }

        foreach (var localTransition in localTransitionList)
        {
            if (localTransition.CanTransition(context))
                currentTransition = CheckPriority(currentTransition, localTransition);
        }

        if (currentTransition != null)
        {
            transition = currentTransition;
            return true;
        }
        
        return false;
    }

    private static StateTransition<TState, TContext> CheckPriority(
        StateTransition<TState, TContext> currentTransition, 
        StateTransition<TState, TContext> targetTransition)
    {
        if (currentTransition == null)
        {
            currentTransition = targetTransition;
            return currentTransition;
        }

        if (currentTransition.Priority < targetTransition.Priority)
            currentTransition = targetTransition;
        return currentTransition;
    }
}