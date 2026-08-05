using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay 
{
    public class ExitGameState : BaseEventState<ExitGameState>
    {
        public ExitGameState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}