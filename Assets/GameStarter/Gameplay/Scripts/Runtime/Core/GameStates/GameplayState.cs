using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public class GameplayState : BaseEventState<GameplayState>
    {
        public GameplayState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}