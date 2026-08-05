using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public class LoadingState : BaseEventState<LoadingState>
    {
        public LoadingState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}