using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay.Core
{
    public class PauseState : BaseEventState<PauseState>
    {
        public PauseState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}