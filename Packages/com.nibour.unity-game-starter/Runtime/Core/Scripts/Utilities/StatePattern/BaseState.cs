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

        internal abstract void EnterState();
        internal virtual void UpdateState() { }
        internal virtual void ExitState() { }
    }
}