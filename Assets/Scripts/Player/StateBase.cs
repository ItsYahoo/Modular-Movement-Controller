using System;
using UnityEngine;

public abstract class StateBase<TPlayerStates> where TPlayerStates : Enum
{
    protected StateBase(TPlayerStates stateKey) => StateKey = stateKey;
    public TPlayerStates StateKey { get; private set; } 
    public virtual void EnterState() { } 
    public virtual TPlayerStates ReturnNewState() { return StateKey; }
    public virtual void ExitState() { } 
    public virtual void TickState() { } 
    public virtual void FixedTickState() { }
    
}
