using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public class GameplayState : BaseEventState<GameplayState>
    {
        public GameplayState(StateMachine stateMachine) : base(stateMachine) { }
    }
}