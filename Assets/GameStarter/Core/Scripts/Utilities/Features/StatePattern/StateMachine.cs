using System;
using UnityEngine;
using UnityGameStarter.TypeStatics;

namespace UnityGameStarter.FiniteStateMachine
{
    /// <summary>
    /// A basic state machine for FSM-driven logic, which the states are pure C# classes 
    /// inherited by <see cref="BaseState"/>
    /// </summary>
    /// <remarks>
    /// This state machine does not support MonoBehaviour states, to make it work 
    /// use <see cref="MonoBehaviourStateMachine"/> instead
    /// </remarks>
    public abstract class StateMachine : BaseStateMachine<BaseState>
    {
        /// <summary>
        /// Keep track of the states by implementing this type array property
        /// </summary>
        protected abstract Type[] InitialStates { get; }

        #region Life Cycle
        protected override void Awake()
        {
            states.Clear();
            
            foreach (var stateType in InitialStates)
            {
                if (!TypeLibrary.IsSubclassOf<BaseState>(stateType)) continue;

                var state = (BaseState)TypeLibrary.CreateInstance(stateType, this);
                states.Add(stateType, state);
            }
        }
        
        protected override void Update()
        {
            currentState?.UpdateState();
        }
        #endregion

        #region API
        public override bool TryGetStateByType<T>(out T state)
        {
            if (!states.TryGetValue(typeof(T), out var foundState)) 
            {
                state = null;
                Debug.LogError($"Does not contain type of {typeof(T)} in the State Machine");
                return false;
            }

            state = foundState as T;
            return true;
        }

        public override void SetState(BaseState newState, bool reset = false)
        {
            if (currentState != null && currentState.Lock) return;
            
            if (newState == null)
            {
                Debug.LogError("State Machine: Unassigned new state detected!");
                return;
            }

            if (!reset && currentState == newState) return;

            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public override void SetState<T>(bool reset = false)
        {
            if (!states.TryGetValue(typeof(T), out var state)) 
            {
                Debug.LogError($"Does not contain type of {typeof(T)} in the State Machine");
                return;
            }

            SetState(state, reset);
        }
        #endregion
    }
}