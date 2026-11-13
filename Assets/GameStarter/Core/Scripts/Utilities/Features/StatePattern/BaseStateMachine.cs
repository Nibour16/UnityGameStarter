using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGameStarter.FiniteStateMachine 
{
    public abstract class BaseStateMachine<TState> : MonoBehaviour
    {
        /// <summary>
        /// Collection of states through a dictionary with the state class type as the key
        /// </summary>
        protected readonly Dictionary<Type, TState> states = new();

        protected TState currentState;
        public virtual TState CurrentState 
        {
            get => currentState;
            protected set => currentState = value;
        }

        /// <summary>
        /// Should set the first/default state during Unity Start
        /// </summary>
        protected virtual bool SetDefault => true;

        #region Life Cycle
        /// <summary>
        /// Override this method to initialize <see cref="states"/>
        /// </summary>
        protected abstract void Awake();

        protected virtual void Start()
        {
            if (SetDefault)
                SetState(states.Values.FirstOrDefault());
        }

        protected abstract void Update();
        #endregion

        public abstract bool TryGetStateByType<T>(out T state) where T : TState;
        public abstract void SetState(TState state, bool reset = false);
        public abstract void SetState<T>(bool reset = false) where T : TState;
    }
}