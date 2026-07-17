using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGameStarter.FiniteStateMachine
{
    using UnityGameStarter.TypeLibrary;
    
    public abstract class BaseStateMachine : MonoBehaviour
    {
        private Dictionary<Type, BaseState> _states = new();
        
        private BaseState _currentState;
        public BaseState CurrentState => _currentState;

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
        public bool TryGetStateByType<T>(out T state) where T : BaseState
        {
            if (!_states.TryGetValue(typeof(T), out var foundState)) 
            {
                state = null;
                Debug.LogError($"Does not contain type of {typeof(T)} in the State Machine");
                return false;
            }

            state = foundState as T;
            return true;
        }

        public void SetState(Type stateType)
        {
            if (!_states.TryGetValue(stateType, out var state)) 
            {
                Debug.LogError($"Does not contain type of {stateType} in the State Machine");
                return;
            }

            SetState(state);
        }

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

        private void ConstructStates(Type[] stateTypes)
        {
            _states = new Dictionary<Type, BaseState>();

            foreach (var stateType in stateTypes)
            {
                if (!TypeLibrary.IsSubclassOf<BaseState>(stateType)) continue;

                var state = (BaseState)TypeLibrary.CreateInstance(stateType, this);
                _states.Add(stateType, state);
            }
        }
    }
}