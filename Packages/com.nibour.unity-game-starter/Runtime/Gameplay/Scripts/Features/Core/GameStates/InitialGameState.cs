using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay 
{
    public class InitialGameState : BaseEventState<InitialGameState>
    {
        public InitialGameState(StateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            base.OnEnter();

            stateMachine.SetState<LoadingState>();
        }
    }
}