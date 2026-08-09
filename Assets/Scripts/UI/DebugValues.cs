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
        
        groundedTrue.SetActive(playerStateMachine.playerStateData.GroundDetector.isGrounded);
        groundedFalse.SetActive(!playerStateMachine.playerStateData.GroundDetector.isGrounded);
        
        speed.text = playerStateMachine.playerStateData.currentSpeed.ToString("F2") + " m/s";
        ChangeBarValue(playerStateMachine.playerStateData.currentSpeed, 
            playerStateMachine.playerStateData.MovementSettings.GetRunSpeed(), 
            speedBar);
        
        stamina.text = playerStateMachine.staminaResource.GetCurrentStamina().ToString("F1") + " / " + playerStateMachine.playerStateData.MovementSettings.GetMaxStamina();
        ChangeBarValue(playerStateMachine.staminaResource.GetCurrentStamina(), 
            playerStateMachine.playerStateData.MovementSettings.GetMaxStamina(), 
            staminaBar);
        
        dashTime.text = "TODO";
        
        slopeAngle.text = playerStateMachine.playerStateData.GroundDetector.slopeAngle.ToString("F2") + " °";
        ChangeBarValue(playerStateMachine.playerStateData.GroundDetector.slopeAngle, 
            360, 
            slopeBar);
        
        velocity.text = playerStateMachine.playerStateData.currentVelocity.ToString("F1");
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
