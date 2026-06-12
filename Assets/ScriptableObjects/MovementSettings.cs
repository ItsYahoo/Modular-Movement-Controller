using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "MoMohammed/MovementSettings", order = 1)]
[Serializable]
public class MovementSettings : ScriptableObject
{
    [Header("Ground Settings")]
    [SerializeField] [Range(0.1f, 30f)] private float acceleration = 18.5f;
    [SerializeField] [Range(0.1f, 30f)] private float deceleration = 18.5f;
    [SerializeField] [Range(0.1f, 5f)] private float turnSmoothTime = 0.1f;
    [SerializeField] [Range(0.0f, 1.0f)] private float EnvironmentMultiplier = 1f;
    [SerializeField] [Range(1f, 10f)] private float walkSpeed = 3.5f;
    [SerializeField] [Range(1f, 15f)] private float runSpeed = 7.5f;
    [SerializeField] [Range(2f, 20f)] private float dashSpeed = 10f;
    [SerializeField] [Range(0.1f, 5f)] private float dashDuration = 0.15f;
    [SerializeField] [Range(0.1f, 10f)] private float dashCooldown = 1.5f;
    [SerializeField] [Range(-20f, -0.1f)] private float gravity = -15.85f;
    [SerializeField] [Range(0.0f, 20f)] private float groundStickForce = 8.5f;
    
    [Header("Air Settings")]
    [SerializeField] [Range(0.1f, 5f)] private float jumpForce = 0.5f;
    [SerializeField] [Range(0.1f, 5f)] private float airControl = 0.5f;

    #region Getters

    public float GetAcceleration() { return acceleration; }
    public float GetDeceleration() { return deceleration; }
    public float GetTurnSmoothTime() { return turnSmoothTime; }
    public float GetEnvironmentMultiplier() { return EnvironmentMultiplier; }
    public float GetWalkSpeed() { return walkSpeed; }
    public float GetRunSpeed() { return runSpeed; }
    public float GetDashSpeed() { return dashSpeed; }
    public float GetDashDuration() { return dashDuration; }
    public float GetDashCooldown() { return dashCooldown; }
    public float GetGravity() { return gravity; }
    public float GetGroundStickForce() { return groundStickForce; }
    public float GetJumpForce() { return jumpForce; }
    public float GetAirControl() { return airControl; }

    #endregion
}
