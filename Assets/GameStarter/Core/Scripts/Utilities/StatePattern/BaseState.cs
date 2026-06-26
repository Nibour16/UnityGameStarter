namespace UnityGameStarter.FiniteStateMachine 
{
    public abstract class BaseState
    {
        private readonly BaseStateMachine _stateMachine;

        public BaseState(BaseStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected T GetStateMachine<T>() where T : BaseStateMachine => _stateMachine as T;

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}