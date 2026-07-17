using UnityEngine;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay.Core
{
    public class GameplayState : BaseEventState<GameplayState>
    {
        public GameplayState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}