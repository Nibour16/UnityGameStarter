using UnityGameStarter.ApplicationStatics;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay 
{
    public class ExitGameState : BaseEventState<ExitGameState>
    {
        public ExitGameState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override bool Lock => true;

        public override void EnterState()
        {
            if (EventManager.Instance.GetListenerCount<EnterStateEvent<ExitGameState>>() <= 0)
                ApplicationLibrary.QuitApp();
            else
                base.EnterState();
        }
    }
}