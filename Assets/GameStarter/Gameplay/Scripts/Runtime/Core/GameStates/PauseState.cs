using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public class PauseState : BaseEventState<PauseState>
    {
        public PauseState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}