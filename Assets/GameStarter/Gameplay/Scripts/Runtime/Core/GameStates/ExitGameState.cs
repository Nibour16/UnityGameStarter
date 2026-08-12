using UnityGameStarter.ApplicationStatics;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;
using UnityGameStarter.SceneManagement;

namespace UnityGameStarter.Gameplay 
{
    public class ExitGameState : BaseEventState<ExitGameState>
    {
        public override bool Lock => true;

        public ExitGameState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            var reason = GetStateMachine<GameStateMachine>().reason;

            if (EventManager.Instance.GetListenerCount<EnterStateEvent<ExitGameState>>() <= 0)
            {
                if (reason == ExitGameReason.Exit)
                    ApplicationLibrary.QuitApp();
                else
                    SceneFacade.Instance.Reload();
            }
            else
                EventManager.Instance.Publish(new EnterGameExitEvent(reason));
        }
    }
}