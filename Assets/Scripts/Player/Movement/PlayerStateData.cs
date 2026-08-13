using Unity.Cinemachine;
using UnityEngine;

public class PlayerStateData
{
    // OUTSIDE STATE VARIABLES
    public PlayerMovementStateMachine MovementStateMachine { get; private set; }
    public SpeedLinesController SpeedLinesController { get; private set; }
    public MovementSettings MovementSettings { get; private set; }
    public GroundDetector GroundDetector { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    public Camera MainCamera { get; private set; }
    public CinemachineCamera CinemachineCamera { get; private set; }
    public CinemachineImpulseSource ImpulseSource { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public StaminaResource StaminaResource { get; private set; }
    
    // IN STATE VARIABLES
    public float currentSpeed { get; set; }
    public Vector3 currentVelocity { get; set; }
    public Vector3 currentHorizontalVelocity { get; set; }
    public float verticalVelocity { get; set; }
    public bool ignoreGroundStickForce { get; set; }
    public float dashCooldown { get; set; }
    
    public PlayerStateData(
        PlayerMovementStateMachine movementStateMachine,
        MovementSettings movementSettings,
        SpeedLinesController speedLinesController,
        GroundDetector groundDetector,
        CharacterController characterController,
        Animator animator,
        Camera mainCamera,
        CinemachineCamera cinemachineCamera,
        Transform playerTransform,
        StaminaResource staminaResource)
    {
        MovementStateMachine = movementStateMachine;
        MovementSettings = movementSettings;
        SpeedLinesController = speedLinesController;
        GroundDetector = groundDetector;
        CharacterController = characterController;
        Animator = animator;
        MainCamera = mainCamera;
        CinemachineCamera = cinemachineCamera;
        PlayerTransform = playerTransform;
        StaminaResource = staminaResource;
        
        ImpulseSource = groundDetector.GetFeetObject().GetComponent<CinemachineImpulseSource>();
    }
}
