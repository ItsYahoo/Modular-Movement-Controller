using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private float barFillRate = 3f;
    [SerializeField] private PlayerMovementStateMachine playerMovement;

    private void Update()
    {
        float currentStamina = playerMovement.staminaResource.GetCurrentStamina();
        float maxStamina = playerMovement.staminaResource.movementSettings.GetMaxStamina();
        float normalizedStamina = Mathf.Clamp01(currentStamina / maxStamina);

        staminaBar.fillAmount = Mathf.MoveTowards(
            staminaBar.fillAmount,
            normalizedStamina,
            barFillRate * Time.deltaTime
        );
    }
}