using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateTransition<TState, TContext> where TState : Enum
{
    public TState TargetState { get; }
    public int Priority { get; }
    private readonly List<IStateCondition<TContext>> conditionList = new();

    public StateTransition(TState targetState, int priority)
    {
        TargetState = targetState;
        Priority = priority;
    }
    
    public bool AddCondition(IStateCondition<TContext> condition)
    {
        if (conditionList.Contains(condition))
            return false;
        
        conditionList.Add(condition);
        return true;
    }

    public bool CanTransition(TContext context)
    {
        return conditionList.All(condition => condition.Evaluate(context));
    }
}