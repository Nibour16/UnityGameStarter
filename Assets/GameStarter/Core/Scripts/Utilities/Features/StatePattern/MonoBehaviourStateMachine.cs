using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGameStarter.UnityObjectStatics;

namespace UnityGameStarter.FiniteStateMachine 
{
    public class MonoBehaviourStateMachine : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] states;
        private readonly List<MonoBehaviour> _stateList = new();

        protected virtual bool SetDefault => true;
        protected virtual bool AllowInheritances => false;

        /// <summary>
        /// Lazy updated field
        /// </summary>
        private MonoBehaviour _currentState;

        /// <summary>
        /// Resolved field, makes it more accurate
        /// </summary>
        public MonoBehaviour CurrentState 
        {
            get 
            {
                if (!_currentState || !_currentState.isActiveAndEnabled) 
                {
                    if (!isActiveAndEnabled) return null;

                    return _currentState = _stateList.FirstOrDefault(x => x && x.isActiveAndEnabled);
                }

                return _currentState;
            }

            private set => _currentState = value;
        }

        protected virtual void Awake() 
        {
            foreach (var state in states) 
            {
                if (!state) 
                {
                    Debug.LogWarning("An invalid state is detected", this);
                    continue;
                }
                
                if (state is MonoBehaviourStateMachine stateMachine &&
                    stateMachine.HasCircularReference(this, x => x.states.OfType<MonoBehaviourStateMachine>())) 
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
                _stateList.Add(state);
            }
        }

        protected virtual void Start() 
        {
            if (SetDefault)
                SetState(_stateList.FirstOrDefault());
        }

        protected virtual void Update() 
        {
            var activeState = _stateList.FirstOrDefault(x => x && x.isActiveAndEnabled);

            if (activeState != _currentState)
                CurrentState = activeState;

            foreach (var state in _stateList)
            {
                if (state != CurrentState && state.enabled)
                    state.enabled = false;
            }
        }

        public void SetState(MonoBehaviour state) 
        {
            if (!state || !_stateList.Contains(state)) 
            {
                Debug.LogError("The target state is not found", this);
                return;
            }

            if (state == CurrentState) return;

            foreach (var itemState in _stateList)
            {
                if (itemState != state && itemState.enabled)
                    itemState.enabled = false;
            }

            CurrentState = state;
            state.enabled = true;
        }

        public void SetState<T>() where T : MonoBehaviour
        {
            var state = GetStateBehaviour<T>();

            SetState(state);
        }

        private bool IsStateDuplicated(MonoBehaviour state) 
        {
            Type targetType = state.GetType();

            return _stateList.Any(x =>
            {
                Type existingType = x.GetType();

                return AllowInheritances? targetType == existingType : 
                    targetType.IsAssignableFrom(existingType) || existingType.IsAssignableFrom(targetType);
            });
        }

        public T GetStateBehaviour<T>() where T : MonoBehaviour
        {
            var state = (T)_stateList.Find(x => x is T);

            if (state == null) 
            {
                Debug.LogError("The target state is not found", this);
                return null;
            }

            return state;
        }
    }
}