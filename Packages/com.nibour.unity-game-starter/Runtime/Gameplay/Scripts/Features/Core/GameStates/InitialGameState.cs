using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay 
{
    public class InitialGameState : BaseEventState<InitialGameState>
    {
        public InitialGameState(StateMachine stateMachine) : base(stateMachine) { }

        internal override void EnterState()
        {
            base.EnterState();

            stateMachine.SetState<LoadingState>();
        }
    }
}