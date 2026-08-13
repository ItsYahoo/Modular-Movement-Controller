using System;
using UnityEngine;

public abstract class StateBase<TStates> where TStates : Enum
{
    protected StateBase(TStates stateKey) => StateKey = stateKey;
    public TStates StateKey { get; private set; } 
    public virtual void EnterState() { } 
    public virtual void ExitState() { } 
    public virtual void TickState() { } 
    public virtual void FixedTickState() { }
    public virtual void LateTickState() { }
    
}
