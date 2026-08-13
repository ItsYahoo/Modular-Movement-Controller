using System;
using System.Collections.Generic;

public class StateGroup<TState> where TState : Enum
{
    // ========= Mainly used for Debugging ========= \
    public string Name { get;  }
    private readonly HashSet<TState> stateList = new();

    public StateGroup(string name)
    {
        Name = name;
    }

    public void AddState(TState state)
    {
        stateList.Add(state);
    }

    public bool Contains(TState state)
    {
        return stateList.Contains(state);
    }
}