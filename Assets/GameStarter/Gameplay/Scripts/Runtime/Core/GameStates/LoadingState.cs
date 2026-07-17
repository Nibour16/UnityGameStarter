using UnityEngine;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay.Core
{
    public class LoadingState : BaseEventState<LoadingState>
    {
        public LoadingState(BaseStateMachine stateMachine) : base(stateMachine) { }
    }
}