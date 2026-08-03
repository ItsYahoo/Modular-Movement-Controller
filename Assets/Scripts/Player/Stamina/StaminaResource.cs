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
    private float regenBuffer; // How much to regen when stamina reaches 0 before unlocking abilities.
    
    // Check and update the stamina loop
    public void TickStamina(float deltaTime)
    {
        if (currentRegenTimer > 0f)
        {
            currentRegenTimer -= deltaTime;
            return;
        }

        if (regenBuffer > 0f)
        {
            if (!movementSettings.IsFillToMax())
                regenBuffer -= deltaTime;
            else if (currentStamina >= movementSettings.GetMaxStamina())
                regenBuffer = 0f;
        }

        // If the current stamina is less than the max stamina
        // then slowly add stamina at the regen rate without overfilling the bar.
        if (currentStamina < movementSettings.GetMaxStamina())
        {
            currentStamina += movementSettings.GetRegenRate() * deltaTime;
            currentStamina = Mathf.Min(currentStamina, movementSettings.GetMaxStamina());
        }
    }

    // Spend a set amount of stamina once.
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

    // Slowly drain set amount of stamina per second.
    // Optional 'ToEmpty' parameter lets the player use the leftover
    // stamina even if they can't "afford it", allowing the stamina bar to drain to 0.
    public bool Drain(float amountPerSecond, float deltaTime, bool toEmpty = false)
    {
        if (currentStamina <= 0f || regenBuffer > 0f)
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
    
    #region Helper Functions

    public bool CanAfford(float amount)
    {
        if (regenBuffer > 0f)
            return false;

        return currentStamina >= amount;
    }
    
    private void ResetRegenDelay() => currentRegenTimer = movementSettings.GetRegenDelay();
    private void ResetRegenBuffer() => regenBuffer = movementSettings.GetRegenBuffer();
    public float GetCurrentStamina() =>  currentStamina;
    public float GetCurrentRegenBuffer() => regenBuffer;

    #endregion
}