using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    // Make this class a Singleton
    public static PlayerInputReader instance;
    public PlayerInput playerInput { get; private set; }
    public Vector2 moveInput { get; private set; }
    public bool sprintHeld { get; private set; }

    private void Start()
    {
        instance = this;
        
        playerInput = new PlayerInput();
        EnablePlayerInputs();
    }

    private void OnDisable()
    {
        if (playerInput != null)
            DisablePlayerInputs();
        
        instance = null;
    }

    #region Actions
    
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    private void OnSprintInput(InputAction.CallbackContext context)
    {
        sprintHeld = context.ReadValueAsButton();
    }
    
    private void DisablePlayerInputs()
    {
        playerInput.Disable();
        playerInput.Player.Movement.performed -= OnMoveInput;
        playerInput.Player.Movement.canceled -= OnMoveInput;
        playerInput.Player.Sprint.performed -= OnSprintInput;
        playerInput.Player.Sprint.canceled -= OnSprintInput;
    }

    private void EnablePlayerInputs()
    {
        playerInput.Enable();
        playerInput.Player.Movement.performed += OnMoveInput;
        playerInput.Player.Movement.canceled += OnMoveInput;
        playerInput.Player.Sprint.performed += OnSprintInput;
        playerInput.Player.Sprint.canceled += OnSprintInput;
    }

    #endregion
    
}
