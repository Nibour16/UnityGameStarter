using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public class LoadingState : BaseEventState<LoadingState>
    {
        public LoadingState(StateMachine stateMachine) : base(stateMachine) { }
    }
}