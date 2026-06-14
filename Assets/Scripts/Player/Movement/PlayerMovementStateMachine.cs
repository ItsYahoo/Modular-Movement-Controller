using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementStateMachine : StateManager<PlayerMovementStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Walk,
        Run
    }

    [SerializeField] private MovementSettings movementSettings;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    private PlayerStateData playerStateData;

    public new void Start()
    {
        InitializeData();
        InitializeStates();
        
        base.Start();
    }

    private void InitializeStates()
    {
        states.Add(PlayerStates.Idle, new PlayerIdleState(playerStateData, PlayerStates.Idle));
        states.Add(PlayerStates.Walk, new PlayerWalkState(playerStateData, PlayerStates.Walk));
        states.Add(PlayerStates.Run, new PlayerRunState(playerStateData, PlayerStates.Run));
        
        currentState = states[PlayerStates.Idle];
    }

    private void InitializeData()
    {
        playerStateData = new PlayerStateData(
            this,
            movementSettings,
            groundDetector,
            characterController,
            animator,
            Camera.main,
            transform
        );
    }
}
