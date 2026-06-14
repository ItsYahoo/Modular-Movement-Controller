using UnityEngine;

public class PlayerStateData
{
    // OUTSIDE STATE VARIABLES
    public PlayerMovementStateMachine MovementStateMachine { get; private set; }
    public MovementSettings MovementSettings { get; private set; }
    public GroundDetector GroundDetector { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    public Camera MainCamera { get; private set; }
    public Transform PlayerTransform { get; private set; }
    
    // IN STATE VARIABLES
    public float currentSpeed { get; set; }
    public Vector3 currentVelocity { get; set; }
    public Vector3 currentHorizontalVelocity { get; set; }
    public float verticalVelocity { get; set; }
    
    public PlayerStateData(
        PlayerMovementStateMachine movementStateMachine,
        MovementSettings movementSettings,
        GroundDetector groundDetector,
        CharacterController characterController,
        Animator animator,
        Camera mainCamera,
        Transform playerTransform)
    {
        MovementStateMachine = movementStateMachine;
        MovementSettings = movementSettings;
        GroundDetector = groundDetector;
        CharacterController = characterController;
        Animator = animator;
        MainCamera = mainCamera;
        PlayerTransform = playerTransform;
    }
}
