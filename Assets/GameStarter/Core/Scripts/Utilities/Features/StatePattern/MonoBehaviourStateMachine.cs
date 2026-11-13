using System;
using System.Linq;
using UnityEngine;
using UnityGameStarter.UnityObjectStatics;

namespace UnityGameStarter.FiniteStateMachine 
{
    public interface IStateBehaviour { }
    
    /// <summary>
    /// A type of state machine that supports any MonoBehaviours as states, 
    /// but they are required to implement <see cref="IStateBehaviour"/>.
    /// 
    /// This state machine supports HFSM, which means that the state can also be a state machine.
    /// </summary>
    /// <remarks>
    /// Since the states are MonoBehaviours, which it won't support pure C# states like 
    /// <see cref="BaseState"/> from <see cref="BaseStateMachine"/>
    /// </remarks>
    public class MonoBehaviourStateMachine : BaseStateMachine<MonoBehaviour>, IStateBehaviour
    {
        [SerializeField] private MonoBehaviour[] initialStates;

        /// <summary>
        /// Resolved property of current state, makes it more accurate
        /// </summary>
        public override MonoBehaviour CurrentState 
        {
            get 
            {
                if (!currentState || !currentState.isActiveAndEnabled) 
                {
                    if (!isActiveAndEnabled) return null;

                    SyncCurrentState();
                }

                return currentState;
            }

            protected set => base.CurrentState = value;
        }

        #region Life Cycle
        protected override void Awake() 
        {
            if (initialStates.Length <= 0) 
            {
                Debug.LogError("States are not implemented in inspector", this);
                return;
            }
            
            foreach (var state in initialStates) 
            {
                if (!state) 
                {
                    Debug.LogWarning("An invalid state is detected", this);
                    continue;
                }

                if (state is not IStateBehaviour)
                {
                    Debug.LogWarning("The target component does not implement IStateBehaviour", this);
                    continue;
                }

                if (state is MonoBehaviourStateMachine stateMachine && stateMachine.HasCircularReference(
                    this, x => x.initialStates.OfType<MonoBehaviourStateMachine>())) 
                {
                    Debug.LogWarning("The state cannot be self", this);
                    continue;
                }

                if (IsStateDuplicated(state)) 
                {
                    Debug.LogWarning("An duplicated state type is detected", this);
                    continue;
                }

                state.enabled = false;
                states.Add(state.GetType(), state);
            }
        }

        protected virtual void OnEnable() 
        {
            if (currentState)
                currentState.enabled = true;
        }

        protected virtual void OnDisable() 
        {
            SyncCurrentState();

            foreach (var state in states.Values)
            {
                if (state.enabled)
                    state.enabled = false;
            }
        }

        protected override void Update() 
        {
            SyncCurrentState();

            foreach (var state in states.Values)
            {
                if (state != CurrentState && state.enabled)
                    state.enabled = false;
            }
        }
        #endregion

        #region Tool Methods
        /// <summary>
        /// Update <see cref="_currentState"/> to the actual result
        /// </summary>
        private void SyncCurrentState()
        {
            var activeState = states.Values.FirstOrDefault(x => x && x.isActiveAndEnabled);

            if (activeState != currentState)
                currentState = activeState;
        }

        private bool IsStateDuplicated(MonoBehaviour state)
        {
            Type targetType = state.GetType();

            return states.Any(x =>
            {
                Type existingType = x.Key;

                return targetType == existingType;
            });
        }
        #endregion

        #region API
        public override bool TryGetStateByType<T>(out T state)
        {
            state = null;

            if (!states.TryGetValue(typeof(T), out var monoState))
            {
                Debug.LogError("The target state is not found", this);
                return false;
            }

            state = (T)monoState;
            return true;
        }

        public override void SetState(MonoBehaviour state, bool reset = false) 
        {
            if (!state || !states.TryGetValue(state.GetType(), out var registeredState) ||
                registeredState != state)
            {
                Debug.LogError("The target state is not found", this);
                return;
            }

            if (state == CurrentState)
            {
                if (!reset) return;

                state.enabled = false;
                state.enabled = true;
                return;
            }

            foreach (var itemState in states.Values)
            {
                if (itemState != state && itemState.enabled)
                    itemState.enabled = false;
            }

            CurrentState = state;
            state.enabled = true;
        }

        public override void SetState<T>(bool reset = false)
        {
            if (TryGetStateByType<T>(out var state))
                SetState(state, reset);
        }
        #endregion
    }
}