using UnityGameStarter.TypeStatics;

namespace UnityGameStarter.FiniteStateMachine 
{
    public abstract class BaseState
    {
        protected readonly BaseStateMachine stateMachine;

        public virtual bool Lock => false;

        public BaseState(BaseStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected T GetStateMachine<T>() where T : BaseStateMachine => stateMachine as T;

        protected bool GetStateMachine<T>(out T stateMachine) where T : BaseStateMachine
            => TypeLibrary.TryCast(this.stateMachine, out stateMachine);

        public abstract void EnterState();
        public abstract void UpdateState();
        public virtual void ExitState() { }
    }
}