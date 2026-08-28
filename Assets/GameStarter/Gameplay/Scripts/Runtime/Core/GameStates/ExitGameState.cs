using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameStarter.ApplicationStatics;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;
using UnityGameStarter.Pool;
using UnityGameStarter.SceneManagement;

namespace UnityGameStarter.Gameplay 
{
    public class ExitGameState : BaseEventState<ExitGameState>
    {
        public override bool Lock => true;

        public ExitGameState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            if (PoolManager.TryGetInstance(out var instance))
                instance.Clear();

            bool useDefault = GetStateMachine<GameStateMachine>().AlwaysUseDefaultExit;

            var reason = GetStateMachine<GameStateMachine>().reason;
            var nextScene = GetStateMachine<GameStateMachine>().nextTargetScene;

            if (EventManager.Instance.GetListenerCount<EnterGameExitEvent>() <= 0 || useDefault)
                DefaultExitScene(reason, nextScene);
            else
                EventManager.Instance.Publish(new EnterGameExitEvent(reason, nextScene));
        }

        /// <summary>
        /// Optional protection to prevent user forgets to place level manager
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="targetScene"></param>
        private void DefaultExitScene(ExitGameReason reason, Scene targetScene) 
        {
            switch (reason)
            {
                case ExitGameReason.OpenNewLevel:
                    SceneFacade.Instance.Load(targetScene);
                    break;

                case ExitGameReason.Restart:
                    SceneFacade.Instance.Reload();
                    break;

                case ExitGameReason.Exit:
                    ApplicationLibrary.QuitApp();
                    break;

                default:
                    Debug.LogWarning(
                        $"ExitGameState: A non-implemented reason '{reason}' has been detected");
                    break;
            }
        }
    }
}