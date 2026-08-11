using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay 
{
    public class InitialGameState : BaseEventState<InitialGameState>
    {
        public InitialGameState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();

            stateMachine.SetState(typeof(LoadingState));
        }
    }
}