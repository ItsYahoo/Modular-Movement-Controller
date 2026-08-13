using System;
using System.Collections.Generic;
using UnityEngine;

public class TransitionRegistry<TState, TContext> where TState : Enum
{
    private readonly Dictionary<TState, List<StateTransition<TState, TContext>>> LocalTransitions = new();
    private readonly List<StateTransition<TState, TContext>> GlobalTransitionList = new();
    private readonly List<StateTransitionSet<TState, TContext>> GroupTransitionList = new();

    public void AddLocal(TState sourceState, StateTransition<TState, TContext> transition)
    {
        if (!LocalTransitions.TryGetValue(sourceState, out var sourceTransitionList))
        {
            sourceTransitionList = new List<StateTransition<TState, TContext>>();
            LocalTransitions.Add(sourceState, sourceTransitionList);
        }
        
        sourceTransitionList.Add(transition);
    }

    public void AddGroup(StateTransitionSet<TState, TContext> transitionSet)
    {
        if (!GroupTransitionList.Contains(transitionSet))
            GroupTransitionList.Add(transitionSet);
        else
            Debug.LogError($"Transition {transitionSet} had an issue adding to the GROUP transition list.");
    }

    public void AddGlobal(StateTransition<TState, TContext> transition)
    {
        if (!GlobalTransitionList.Contains(transition))
            GlobalTransitionList.Add(transition);
        else
            Debug.LogError($"Transition {transition.TargetState} had an issue adding to the GLOBAL transition list.");
    }

    public IReadOnlyList<StateTransition<TState, TContext>> GetLocalTransitions(TState state)
    {
        if (LocalTransitions.TryGetValue(state, out var sourceTransitionList))
            return sourceTransitionList;

        return Array.Empty<StateTransition<TState, TContext>>();
    }

    public IReadOnlyList<StateTransition<TState, TContext>> GetGroupTransitions(TState state)
    {
        List<StateTransition<TState, TContext>> sourceTransitionList = new();
        
        foreach (var groupTransition in GroupTransitionList)
        {
            if (!groupTransition.Contains(state)) 
                continue;
            
            foreach (var stateTransition in groupTransition.GetTransitions())
            {
                sourceTransitionList.Add(stateTransition);
            }
        }

        return sourceTransitionList;
    }

public IReadOnlyList<StateTransition<TState, TContext>> GetGlobalTransitions()
        => GlobalTransitionList;
}