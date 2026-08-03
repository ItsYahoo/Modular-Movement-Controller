using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private float barFillRate = 3f;
    [SerializeField] private PlayerMovementStateMachine playerMovement;
    [SerializeField] private Color emptyStaminaBarColor = Color.red;
    [SerializeField] private Color emptyStaminaBarColor2 = Color.orangeRed;
    [SerializeField] private float emptyStaminaFlashRate = 0.25f;

    private Color originalStaminaColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        originalStaminaColor = staminaBar.color;
    }

    private void Update()
    {
        // Get a normalized stamina level (between 0 and 1)
        float currentStamina = playerMovement.staminaResource.GetCurrentStamina();
        float maxStamina = playerMovement.staminaResource.movementSettings.GetMaxStamina();
        float normalizedStamina = Mathf.Clamp01(currentStamina / maxStamina);

        // set the fill amount using MoveTowards to avoid snapping.
        staminaBar.fillAmount = Mathf.MoveTowards(
            staminaBar.fillAmount,
            normalizedStamina,
            barFillRate * Time.deltaTime
        );

        // if the bar is empty, cause the bar to flash red to indicate Exhausted
        if (playerMovement.staminaResource.GetCurrentRegenBuffer() > 0f
            && flashCoroutine == null)
        {
            flashCoroutine = StartCoroutine(FlashStaminaBar());
        }
    }

    IEnumerator FlashStaminaBar()
    {
        while (playerMovement.staminaResource.GetCurrentRegenBuffer() > 0f)
        {
            // 1st color Flash
            staminaBar.color = emptyStaminaBarColor;
            yield return new WaitForSeconds(emptyStaminaFlashRate);
            
            // 2nd color Flash
            staminaBar.color = emptyStaminaBarColor2;
            yield return new WaitForSeconds(emptyStaminaFlashRate);
        }
        
        // Return to Original
        staminaBar.color = originalStaminaColor;
        flashCoroutine = null;
    }
}