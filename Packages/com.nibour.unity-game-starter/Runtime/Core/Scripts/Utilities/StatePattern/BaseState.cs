using UnityGameStarter.TypeStatics;

namespace UnityGameStarter.FiniteStateMachine 
{
    public abstract class BaseState
    {
        protected readonly StateMachine stateMachine;

        public virtual bool Lock => false;

        public BaseState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected T GetStateMachine<T>() where T : StateMachine => stateMachine as T;

        protected bool GetStateMachine<T>(out T stateMachine) where T : StateMachine
            => TypeLibrary.TryCast(this.stateMachine, out stateMachine);

        protected abstract void OnEnter();
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }

        internal void EnterState() => OnEnter();
        internal void UpdateState() => OnUpdate();
        internal void ExitState() => OnExit();
    }
}