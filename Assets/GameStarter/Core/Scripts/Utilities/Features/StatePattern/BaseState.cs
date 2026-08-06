namespace UnityGameStarter.FiniteStateMachine 
{
    public abstract class BaseState
    {
        protected readonly BaseStateMachine stateMachine;

        public BaseState(BaseStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected T GetStateMachine<T>() where T : BaseStateMachine => stateMachine as T;

        public virtual bool Lock => false;

        public abstract void EnterState();
        public abstract void UpdateState();
        public virtual void ExitState() { }
    }
}