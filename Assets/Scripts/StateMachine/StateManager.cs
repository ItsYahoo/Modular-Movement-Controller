using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<TState, TContext> : MonoBehaviour where TState : Enum
{
    protected Dictionary<TState, StateBase<TState>> states = new();
    protected readonly TransitionRegistry<TState, TContext> transitionRegistry = new();
    protected TransitionResolver<TState, TContext> transitionResolver;
    public TContext context;
    public StateBase<TState> currentState { get; set; }
    public StateBase<TState> previousState { get; private set; }
    private bool changingState;

    protected virtual void Awake()
    {
        transitionResolver = new TransitionResolver<TState, TContext>(transitionRegistry);
    }

    protected virtual void Start()
    {
        currentState?.EnterState();
    }
    
    protected virtual void Update()
    {
        if (changingState || currentState == null)
            return;

        if (transitionResolver.TryResolve(currentState.StateKey, context, out var transition))
        {
            ChangeState(transition.TargetState);
            return;
        }

        currentState.TickState();
    }

    protected virtual void FixedUpdate()
    {
        if (changingState || currentState == null)
            return;
        
        currentState.FixedTickState();
    }
    
    public void ChangeState(TState newStateKey)
    {
        if (currentState != null && currentState.StateKey.Equals(newStateKey)) 
            return;
        
        if (!states.TryGetValue(newStateKey, out StateBase<TState> newState))
        {
            Debug.LogError($"State '{newStateKey}' was not registered.");
            return;
        }

        changingState = true;
        previousState = currentState;
        
        previousState?.ExitState();
        currentState = states[newStateKey];
        currentState?.EnterState();
        
        changingState = false;
    }
}
