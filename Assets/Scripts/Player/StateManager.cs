using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<TPlayerStates> : MonoBehaviour where TPlayerStates : Enum
{
    protected Dictionary<TPlayerStates, StateBase<TPlayerStates>> states = new();
    public StateBase<TPlayerStates> currentState { get; set; }
    public StateBase<TPlayerStates> previousState { get; private set; }
    private bool changingState;

    private void Update()
    {
        if (changingState || currentState == null)
            return;
        
        TPlayerStates newStateKey = currentState.ReturnNewState();

        if (!newStateKey.Equals(currentState.StateKey))
        {
            ChangeState(newStateKey);
            return;
        }

        currentState.TickState();
    }

    private void FixedUpdate()
    {
        if (changingState || currentState == null)
            return;
        
        currentState.FixedTickState();
    }
    
    protected void ChangeState(TPlayerStates newStateKey)
    {
        if (currentState != null && currentState.StateKey.Equals(newStateKey)) 
            return;

        changingState = true;
        previousState = currentState;
        
        previousState?.ExitState();
        currentState = states[newStateKey];
        currentState?.EnterState();
        
        changingState = false;
    }
}
