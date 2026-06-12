using System;
using UnityEngine;

public class PlayerMovementStateMachine : StateManager<PlayerMovementStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Walk,
        Run
    }

    private void Start()
    {
        InitializeStates();
    }

    private void InitializeStates()
    {
        // TODO: Add States
        
        currentState = states[PlayerStates.Idle];
    }
}
