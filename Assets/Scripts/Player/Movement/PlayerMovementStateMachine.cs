using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementStateMachine : StateManager<PlayerMovementStateMachine.PlayerStates, PlayerStateData>
{
    public enum PlayerStates
    {
        Dash,
        
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
    
    // Groups
    private StateTransitionSet<PlayerStates, PlayerStateData> groundedGroup;

    public new void Start()
    {
        InitializeData();
        InitializeStates();
        InitializeTransitions();
        InitializeGroups();
        
        base.Start();
    }

    private void InitializeStates()
    {
        states.Add(PlayerStates.Dash, new PlayerDashState(context, PlayerStates.Dash));
        
        // Ground States
        states.Add(PlayerStates.Idle, new PlayerIdleState(context, PlayerStates.Idle));
        states.Add(PlayerStates.Walk, new PlayerWalkState(context, PlayerStates.Walk));
        states.Add(PlayerStates.Run, new PlayerRunState(context, PlayerStates.Run));
        
        // Airborne States
        states.Add(PlayerStates.Jump, new PlayerJumpState(context, PlayerStates.Jump));
        states.Add(PlayerStates.Fall, new PlayerFallState(context, PlayerStates.Fall));
        states.Add(PlayerStates.Land, new PlayerLandState(context, PlayerStates.Land));
        
        currentState = states[PlayerStates.Idle];
    }

    private void InitializeData()
    {
        staminaResource = new StaminaResource(
            movementSettings);
        
        context = new PlayerStateData(
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

    private void InitializeTransitions()
    {
        // Idle State Transitions
        var IdleToWalk = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Walk, 10);
        IdleToWalk.AddCondition(new IsMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Idle, IdleToWalk); // Idle -> Walk

        // Walk State Transitions
        var WalkToIdle = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Idle, 10);
        WalkToIdle.AddCondition(new NotMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Walk, WalkToIdle); // Walk -> Idle
    }

    private void InitializeGroups()
    {
        // Grounded Group
        groundedGroup = new StateTransitionSet<PlayerStates, PlayerStateData>();
        
        groundedGroup.AddState(PlayerStates.Idle);
        groundedGroup.AddState(PlayerStates.Walk);
        groundedGroup.AddState(PlayerStates.Run);
        
        // Grounded -> Fall
        var fallTransition = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Fall,100);
        fallTransition.AddCondition(new IsFallingCondition());
        groundedGroup.AddTransition(fallTransition);
        
        // Register the completed group
        transitionRegistry.AddGroup(groundedGroup);
    }
}
