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
        Vector3 cameraRelativeDirection = GetCameraRelativeInputDirection(inputDirection);
        CalculateCurrentSpeed(inputDirection);
        CalculateCurrentHorizontalVelocity(inputDirection, cameraRelativeDirection);
        CalculateRotation(inputDirection, cameraRelativeDirection);

        Vector3 finalVelocity = new  Vector3(currentHorizontalVelocity.x, currentVelocity.y, currentHorizontalVelocity.z);
        
        currentVelocity = finalVelocity;

        characterController.Move(currentVelocity * Time.deltaTime);
    }

    private void CalculateCurrentHorizontalVelocity(Vector2 inputDirection, Vector3 cameraRelativeDirection)
    {
        Vector3 targetHorizontalVelocity = cameraRelativeDirection * currentSpeed;
        bool hasInput = inputDirection.sqrMagnitude > 0.01f;
        bool hasCurrentVelocity = currentHorizontalVelocity.sqrMagnitude > 0.01f;

        bool changingDirection = false;
        float speedChangeRate;
        
        if (hasInput && hasCurrentVelocity)
        {
            // Check if the player is trying to change direction by comparing the current
            // velocity direction with the camera relative input direction
            Vector3 currentDirection = currentHorizontalVelocity.normalized;
            float directionDot = Vector3.Dot(currentDirection, cameraRelativeDirection);

            changingDirection = directionDot < 0.75f;
        }
        
        if (!hasInput)
            speedChangeRate = movementSettings.GetDeceleration();
        else if (changingDirection)
            speedChangeRate = movementSettings.GetDirectionChangeAcceleration();
        else
            speedChangeRate = movementSettings.GetAcceleration();

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity, targetHorizontalVelocity, speedChangeRate * Time.deltaTime);
    }

    private void CalculateCurrentSpeed(Vector2 inputDirection)
    {
        bool hasInput = inputDirection.sqrMagnitude > 0.01f;
        
        float run = PlayerInputReader.instance.sprintHeld ? 1 : 0; // 1 if run, 0 if walk
        float moveSpeed = (run * movementSettings.GetRunSpeed() + (1 - run) * movementSettings.GetWalkSpeed()) * movementSettings.GetEnvironmentMultiplier();
        float targetSpeed = hasInput ? moveSpeed : 0f;
        
        float speedChangeRate = currentSpeed < targetSpeed
            ? movementSettings.GetAcceleration() : movementSettings.GetDeceleration();

        currentSpeed = Mathf.MoveTowards(
            currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
        
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

    private void CalculateRotation(Vector2 inputDirection, Vector3 cameraRelativeDirection)
    {
        // Don't rotate if there is no input
        if (inputDirection.sqrMagnitude < 0.01f)
            return;

        // Don't rotate if the direction is too small
        if (cameraRelativeDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation, targetRotation, movementSettings.GetTurnSmoothTime() * Time.deltaTime);
    }
}
