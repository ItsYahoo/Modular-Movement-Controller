using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private float barFillRate;
    [SerializeField] private PlayerMovementStateMachine playerMovement;

    private void Update()
    {
        // TODO: Update Fill amount for Stamina UI
    }
}
