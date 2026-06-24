using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> _states = new();
    private BaseState _currentState;

    #region Initialization
    protected virtual void Awake() 
    {
        ConstructStates(GetInitialStates());
    }

    protected virtual void Start()
    {
        SetState(_states.Values.FirstOrDefault());
    }

    protected abstract Type[] GetInitialStates();
    #endregion

    #region Update
    protected virtual void Update() 
    {
        _currentState?.UpdateState();
    }
    #endregion

    #region API
    public void ChangeState(Type stateType) 
    {
        if (!TypeLibrary.IsSubclassOf<BaseState>(stateType)) return;
        
        SetState(_states[stateType]);
    }
    #endregion

    private void ConstructStates(Type[] stateTypes)
    {
        _states = new Dictionary<Type, BaseState>();

        foreach (var stateType in stateTypes)
        {
            if (!TypeLibrary.IsSubclassOf<BaseState>(stateType)) continue;

            var state = (BaseState)TypeLibrary.CreateInstance(stateType);
            _states.Add(stateType, state);
        }
    }

    private void SetState(BaseState newState) 
    {
        if (newState == null) 
        {
            Debug.LogError("State Machine: Unassigned new state detected!");
            return;
        }
        
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
}
