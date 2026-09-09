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
    private StateTransitionSet<PlayerStates, PlayerStateData> airborneGroup;

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

        var IdleToDash = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Dash, 8);
        IdleToDash.AddCondition(new IsDashingCondition());
        transitionRegistry.AddLocal(PlayerStates.Idle, IdleToDash); // Idle -> Dash
        
        // Walk State Transitions
        var WalkToIdle = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Idle, 10);
        WalkToIdle.AddCondition(new NotMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Walk, WalkToIdle); // Walk -> Idle
        
        var WalkToRun = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Run, 8);
        WalkToRun.AddCondition(new IsRunningCondition());
        transitionRegistry.AddLocal(PlayerStates.Walk, WalkToRun); // Walk -> Run
        
        // Run State Transitions
        var RunToIdle = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Idle, 10);
        RunToIdle.AddCondition(new NotMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Run, RunToIdle); // Run -> Idle
        
        var RunToWalk = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Walk, 7);
        RunToWalk.AddCondition(new NotRunningCondition());
        transitionRegistry.AddLocal(PlayerStates.Run, RunToWalk); // Run -> Walk
        
        var RunToWalkStamina = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Walk, 7);
        RunToWalkStamina.AddCondition(new NoStamina());
        transitionRegistry.AddLocal(PlayerStates.Run, RunToWalkStamina); // Run -> Walk
        
        var RunToDash = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Dash, 5);
        RunToDash.AddCondition(new IsDashingCondition());
        transitionRegistry.AddLocal(PlayerStates.Run, RunToDash); // Run -> Dash
        
        // Dash State Transitions
        var dashToWalk = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Walk, 10);
        dashToWalk.AddCondition(new IsNotDashStateCondition());
        dashToWalk.AddCondition(new IsMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Dash, dashToWalk); // Dash -> Walk
        
        var dashToRun = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Run, 8);
        dashToRun.AddCondition(new IsNotDashStateCondition());
        dashToRun.AddCondition(new IsRunningCondition());
        dashToRun.AddCondition(new IsMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Dash, dashToRun); // Dash -> Run
        
        var dashToIdle = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Idle, 6);
        dashToIdle.AddCondition(new IsNotDashStateCondition());
        dashToIdle.AddCondition(new NotMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Dash, dashToIdle); // Dash -> Idle
        
        // Land State Transitions
        var LandToWalk = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Walk, 10);
        LandToWalk.AddCondition(new IsNotLandStateCondition());
        LandToWalk.AddCondition(new IsMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Land, LandToWalk); // Land -> Walk
        
        var LandToRun = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Run, 8);
        LandToRun.AddCondition(new IsNotLandStateCondition());
        LandToRun.AddCondition(new IsRunningCondition());
        LandToRun.AddCondition(new IsMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Land, LandToRun); // Land -> Run
        
        var LandToIdle = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Idle, 6);
        LandToIdle.AddCondition(new IsNotLandStateCondition());
        LandToIdle.AddCondition(new NotMovingCondition());
        transitionRegistry.AddLocal(PlayerStates.Land, LandToIdle); // Land -> Idle
        
        // Jump State Transitions
        var jumpToFall = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Fall, 10);
        jumpToFall.AddCondition(new LeftTheGroundCondition());
        transitionRegistry.AddLocal(PlayerStates.Jump, jumpToFall); // Jump -> Fall
        
        // Fall State Transitions
        var FallToLand = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Land, 100);
        FallToLand.AddCondition(new IsGroundedCondition());
        transitionRegistry.AddLocal(PlayerStates.Fall, FallToLand); // Fall -> Land
        
        var FallToJump = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Jump, 10);
        FallToJump.AddCondition(new HasJumpBufferCondition());
        transitionRegistry.AddLocal(PlayerStates.Fall, FallToJump); // Fall -> Jump
    }

    private void InitializeGroups()
    {
        #region Groups
        // Grounded Group
        groundedGroup = new StateTransitionSet<PlayerStates, PlayerStateData>();
        
        groundedGroup.AddState(PlayerStates.Idle);
        groundedGroup.AddState(PlayerStates.Walk);
        groundedGroup.AddState(PlayerStates.Run);
        groundedGroup.AddState(PlayerStates.Land);
        
        // Airborne Group
        airborneGroup = new StateTransitionSet<PlayerStates, PlayerStateData>();
        
        airborneGroup.AddState(PlayerStates.Fall);
        airborneGroup.AddState(PlayerStates.Jump);
        #endregion

        #region Transitions
        // Grounded -> Fall (Ground Group)
        var fallTransition = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Fall,100);
        fallTransition.AddCondition(new IsFallingCondition());
        groundedGroup.AddTransition(fallTransition);
        
        // Grounded -> Jump (Ground Group)
        var JumpTransition = new StateTransition<PlayerStates, PlayerStateData>(PlayerStates.Jump, 110);
        JumpTransition.AddCondition(new IsJumpingCondition());
        groundedGroup.AddTransition(JumpTransition);
        
        #endregion
        
        // Register the completed group
        transitionRegistry.AddGroup(airborneGroup);
        transitionRegistry.AddGroup(groundedGroup);
    }
}
