using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Scripting;

[RequireComponent(typeof(CharacterController), typeof(GroundDetector))]
public class PlayerMovementMotor : MonoBehaviour
{
    [SerializeField] private MovementSettings movementSettings;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    
    private Vector3 currentVelocity;
    private Vector3 currentHorizontalVelocity;
    private float currentSpeed;
    private Camera mainCamera;
    private float turnSmoothVelocity;

    private void Start()
    {
        mainCamera = Camera.main;
        
        if (groundDetector == null)
            groundDetector = GetComponent<GroundDetector>();
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move(PlayerInputReader.instance.moveInput);
    }

    private void Move(Vector2 inputDirection)
    {
        // Apply gravity
        if (groundDetector.isGrounded)
            currentVelocity.y = -movementSettings.GetGroundStickForce();
        else
            currentVelocity.y += movementSettings.GetGravity() * Time.deltaTime;

        // Calculate Horizontal Velocity
        CalculateCurrentSpeed(inputDirection, out float speedChangeRate);
        CalculateCurrentHorizontalVelocity(inputDirection, speedChangeRate);
        CalculateRotation(inputDirection);

        Vector3 finalVelocity = new  Vector3(currentHorizontalVelocity.x, currentVelocity.y, currentHorizontalVelocity.z);
        
        currentVelocity = finalVelocity;

        characterController.Move(currentVelocity * Time.deltaTime);
    }

    private void CalculateCurrentHorizontalVelocity(Vector2 inputDirection, float speedChangeRate)
    {
        Vector3 cameraRelativeDirection = GetCameraRelativeInputDirection(inputDirection);
        Vector3 targetHorizontalVelocity = cameraRelativeDirection * currentSpeed;

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetHorizontalVelocity,
            speedChangeRate * Time.deltaTime
        );
    }

    private void CalculateCurrentSpeed(Vector2 inputDirection, out float speedChangeRate)
    {
        bool hasInput = inputDirection.sqrMagnitude > 0.01f;
        
        float run = PlayerInputReader.instance.sprintHeld ? 1 : 0; // 1 if run, 0 if walk
        float moveSpeed = (run * movementSettings.GetRunSpeed() + (1 - run) * movementSettings.GetWalkSpeed()) * movementSettings.GetEnvironmentMultiplier();
        
        float targetSpeed = hasInput
            ? moveSpeed
            : 0f;
        
        speedChangeRate = currentSpeed < targetSpeed
            ? movementSettings.GetAcceleration()
            : movementSettings.GetDeceleration();

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            speedChangeRate * Time.deltaTime
        );
        animator.SetFloat("CurrentSpeed", currentSpeed);
    }

    private Vector3 GetCameraRelativeInputDirection(Vector2 inputDirection)
    {
        // Get the camera's forward and right vectors
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        // Project the camera's forward and right vectors onto the horizontal plane
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // Calculate the movement direction relative to the camera
        return (cameraForward * inputDirection.y + cameraRight * inputDirection.x).normalized;
    }

    private void CalculateRotation(Vector2 inputDirection)
    {
        // Don't rotate if there is no input
        if (inputDirection.sqrMagnitude < 0.01f)
            return;

        Vector3 direction = GetCameraRelativeInputDirection(inputDirection);

        // Don't rotate if the direction is too small
        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            movementSettings.GetTurnSmoothTime() * Time.deltaTime
        );
    }
}
