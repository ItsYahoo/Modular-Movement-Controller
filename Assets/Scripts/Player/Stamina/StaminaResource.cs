using UnityEngine;

public class StaminaResource
{
    public StaminaResource(MovementSettings movementSettings)
    {
        this.movementSettings = movementSettings;
        currentStamina = movementSettings.GetMaxStamina();
    }

    public readonly MovementSettings movementSettings;

    private float currentStamina;
    private float currentRegenTimer;
    private float regenBuffer;

    public void TickStamina(float deltaTime)
    {
        if (currentRegenTimer > 0f)
        {
            currentRegenTimer -= deltaTime;
            return;
        }

        if (regenBuffer > 0f)
        {
            regenBuffer -= deltaTime;
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
        currentStamina = Mathf.Max(currentStamina, 0f);

        ResetRegenDelay();

        if (currentStamina <= 0f)
            ResetRegenBuffer();

        return true;
    }

    public bool Drain(float amountPerSecond, float deltaTime, bool toEmpty = false)
    {
        if (currentStamina <= 0f)
            return false;

        currentStamina -= amountPerSecond * deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0f);

        ResetRegenDelay();

        if (currentStamina <= 0f)
        {
            ResetRegenBuffer();
            if (toEmpty)
                return false;
        }

        if (toEmpty)
            return true;
        return currentStamina > 0f;
    }

    public bool CanAfford(float amount)
    {
        if (regenBuffer > 0f)
            return false;

        return currentStamina >= amount;
    }

    private void ResetRegenDelay()
    {
        currentRegenTimer = movementSettings.GetRegenDelay();
    }

    private void ResetRegenBuffer()
    {
        regenBuffer = movementSettings.GetRegenBuffer();
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}