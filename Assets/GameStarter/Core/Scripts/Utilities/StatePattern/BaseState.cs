public abstract class BaseState
{
    protected readonly BaseStateMachine stateMachine;
    
    public BaseState(BaseStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
