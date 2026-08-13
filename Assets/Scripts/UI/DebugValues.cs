using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugValues : MonoBehaviour
{
    [SerializeField] private PlayerMovementStateMachine playerStateMachine;
    [SerializeField] private TextMeshProUGUI currentState;
    [SerializeField] private GameObject groundedTrue;
    [SerializeField] private GameObject groundedFalse;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private Image speedBar;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private Image staminaBar;
    [SerializeField] private TextMeshProUGUI dashTime;
    [SerializeField] private TextMeshProUGUI slopeAngle;
    [SerializeField] private Image slopeBar;
    [SerializeField] private TextMeshProUGUI velocity;
    [SerializeField] private TextMeshProUGUI hasInput;

    private void Update()
    {
        UpdateDebugText();
    }

    private void UpdateDebugText()
    {
        currentState.text = playerStateMachine.currentState.StateKey.ToString();
        
        groundedTrue.SetActive(playerStateMachine.context.GroundDetector.isGrounded);
        groundedFalse.SetActive(!playerStateMachine.context.GroundDetector.isGrounded);
        
        speed.text = playerStateMachine.context.currentSpeed.ToString("F2") + " m/s";
        ChangeBarValue(playerStateMachine.context.currentSpeed, 
            playerStateMachine.context.MovementSettings.GetRunSpeed(), 
            speedBar);
        
        stamina.text = playerStateMachine.staminaResource.GetCurrentStamina().ToString("F1") + " / " + playerStateMachine.context.MovementSettings.GetMaxStamina();
        ChangeBarValue(playerStateMachine.staminaResource.GetCurrentStamina(), 
            playerStateMachine.context.MovementSettings.GetMaxStamina(), 
            staminaBar);
        
        dashTime.text = "TODO";
        
        slopeAngle.text = playerStateMachine.context.GroundDetector.slopeAngle.ToString("F2") + " °";
        ChangeBarValue(playerStateMachine.context.GroundDetector.slopeAngle, 
            360, 
            slopeBar);
        
        velocity.text = playerStateMachine.context.currentVelocity.ToString("F1");
        hasInput.text = PlayerInputReader.instance.moveInput.ToString();
    }

    private void ChangeBarValue(float currentValue, float maxValue, Image bar)
    {
        float normalizedValue = Mathf.Clamp01(currentValue / maxValue);
        
        bar.fillAmount = Mathf.MoveTowards(
            bar.fillAmount,
            normalizedValue,
            3 * Time.deltaTime);
    }
}
