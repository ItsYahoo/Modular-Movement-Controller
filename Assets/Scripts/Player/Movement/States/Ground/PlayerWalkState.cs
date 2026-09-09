using UnityEngine;
using static PlayerMovementStateMachine;

public class PlayerWalkState : PlayerMovementStateBase
{
    public PlayerWalkState(PlayerStateData stateData, PlayerStates stateKey) : base(stateData, stateKey) {}

    public override void EnterState()
    {
        Debug.Log("Entering Walk State");
    }
}