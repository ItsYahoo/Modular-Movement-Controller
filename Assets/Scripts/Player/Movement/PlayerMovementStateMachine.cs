using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementStateMachine : StateManager<PlayerMovementStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        // Grounded States
        Idle,
        Walk,
        Run,
        
        // Airborne States
        Jump,
        Fall,
        Land
    }

    [SerializeField] private MovementSettings movementSettings;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private SpeedLinesController speedLinesController;
    public StaminaResource staminaResource;
    private PlayerStateData playerStateData;

    public new void Start()
    {
        InitializeData();
        InitializeStates();
        
        base.Start();
    }

    private void InitializeStates()
    {
        // Ground States
        states.Add(PlayerStates.Idle, new PlayerIdleState(playerStateData, PlayerStates.Idle));
        states.Add(PlayerStates.Walk, new PlayerWalkState(playerStateData, PlayerStates.Walk));
        states.Add(PlayerStates.Run, new PlayerRunState(playerStateData, PlayerStates.Run));
        
        // Airborne States
        states.Add(PlayerStates.Jump, new PlayerJumpState(playerStateData, PlayerStates.Jump));
        states.Add(PlayerStates.Fall, new PlayerFallState(playerStateData, PlayerStates.Fall));
        states.Add(PlayerStates.Land, new PlayerLandState(playerStateData, PlayerStates.Land));
        
        currentState = states[PlayerStates.Idle];
    }

    private void InitializeData()
    {
        staminaResource = new StaminaResource(
            movementSettings);
        
        playerStateData = new PlayerStateData(
            this,
            movementSettings,
            speedLinesController,
            groundDetector,
            characterController,
            animator,
            Camera.main,
            cinemachineCamera,
            transform,
            staminaResource
        );
    }
}
