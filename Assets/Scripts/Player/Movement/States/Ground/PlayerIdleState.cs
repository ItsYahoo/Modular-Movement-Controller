using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerIdleState : PlayerMovementStateBase
{
    public PlayerIdleState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }
}
