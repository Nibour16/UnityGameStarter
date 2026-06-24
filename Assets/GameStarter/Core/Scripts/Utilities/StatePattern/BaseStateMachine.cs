using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    public readonly Dictionary<System.Type, BaseState> States = new();

    private BaseState _currentState;
    public BaseState CurrentState => _currentState;

    #region Initialization
    protected virtual void Awake() 
    {
        RecordStates(GetInitialStates());
    }

    protected virtual void Start()
    {
        SetState(States.Values.FirstOrDefault());
    }

    protected abstract BaseState[] GetInitialStates();
    #endregion

    private void RecordStates(BaseState[] states)
    {
        foreach (var state in states)
        {
            var type = state.GetType();

            if (!States.ContainsKey(type))
            {
                States.Add(type, state);
            }
        }
    }

    #region Update
    protected virtual void Update() 
    {
        _currentState?.UpdateState();
    }
    #endregion

    #region API
    public void SetState(BaseState newState) 
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
    #endregion
}
