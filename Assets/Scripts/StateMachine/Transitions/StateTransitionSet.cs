using System;
using System.Collections.Generic;

public class StateTransitionSet<TState, TContext> where TState : Enum
{
    private readonly HashSet<TState> states = new();
    private readonly List<StateTransition<TState, TContext>> transitions = new();


    public void AddState(TState state)
    {
        states.Add(state);
    }


    public void AddTransition(StateTransition<TState, TContext> transition)
    {
        transitions.Add(transition);
    }


    public bool Contains(TState state)
    {
        return states.Contains(state);
    }
    
    public IReadOnlyList<StateTransition<TState, TContext>> GetTransitions()
    {
        return transitions;
    }
}