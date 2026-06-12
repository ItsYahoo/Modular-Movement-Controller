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
        float run = 0f; // 1 if run, 0 if walk
        float targetSpeed = (run * movementSettings.GetRunSpeed() + (1 - run) * movementSettings.GetWalkSpeed()) * movementSettings.GetEnvironmentMultiplier();

        float speedChangeRate = currentSpeed < targetSpeed
            ? movementSettings.GetAcceleration()
            : movementSettings.GetDeceleration();

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            speedChangeRate * Time.deltaTime
        );
        
        Vector3 cameraRelativeDirection = GetCameraRelativeInputDirection(inputDirection);
        Vector3 targetHorizontalVelocity = cameraRelativeDirection * targetSpeed;

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetHorizontalVelocity,
            speedChangeRate * Time.deltaTime
        );
        
        Vector3 finalVelocity = new  Vector3(currentHorizontalVelocity.x, currentVelocity.y, currentHorizontalVelocity.z);
        
        currentVelocity = finalVelocity;

        characterController.Move(currentVelocity * Time.deltaTime);
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
}
