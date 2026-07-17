using UnityEditor;
using UnityEngine;

public class StaminaResource
{
    public readonly MovementSettings movementSettings;
    public StaminaResource(MovementSettings _movementSettings)
    {
        movementSettings = _movementSettings;
    }
    
    private float currentStamina;
    private float currentRegenTimer;

    public void TickStamina(float deltaTime)
    {
        if (currentRegenTimer > 0f)
        {
            currentRegenTimer -= deltaTime;
            return;
        }

        if (currentStamina < movementSettings.GetMaxStamina())
        {
            currentStamina += movementSettings.GetRegenRate() * deltaTime;
            currentStamina = Mathf.Min(currentStamina, movementSettings.GetMaxStamina());
        }
    }

    public bool Spend(float amount)
    {
        if (!CanAfford(amount))
            return false;
        
        currentStamina -= amount;
        ResetRegenDelay();
        return true;
    }

    public bool Drain(float amountPerSecond, float DeltaTime)
    {
        if (currentStamina <= 0)
            return false;
        
        currentStamina -= amountPerSecond * DeltaTime;
        currentStamina = Mathf.Max(currentStamina, 0f);

        ResetRegenDelay();
        return currentStamina > 0f;
    }

    public bool CanAfford(float amount)
    {
        return currentStamina >= amount;
    }
    public void ResetRegenDelay()
    {
        currentRegenTimer = movementSettings.GetRegenDelay();
    }
    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}
