using UnityEngine;
using UnityGameStarter.ApplicationStatics;
using UnityGameStarter.CommonData;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.FiniteStateMachine.EventState;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.LevelManagement 
{
    [RequireComponent(typeof(EventListenerRegister))]
    public class LevelManager : Singleton<LevelManager>, IAutoEventListener
    {
        #region Serialized Fields
        [SerializeField] private string mainSceneName;
        [SerializeField] private bool quitIfMissingMainScene = true;
        #endregion

        #region Private Fields with Properties
        private InitializationState _initializationState = InitializationState.Uninitialized;
        public InitializationState InitializationState => _initializationState;

        private int _pendingInitializationCount = 0;
        public int PendingInitializationCount => _pendingInitializationCount;
        #endregion

        #region API
        public void AddInitializationCount() => _pendingInitializationCount++;

        public void RemoveInitializationCount() 
        {
            if (_pendingInitializationCount <= 0) return;
            _pendingInitializationCount--; 
        }
        #endregion

        #region Event Listener Events
        [EventListener]
        private void InitializeLevel(EnterStateEvent<LoadingState> e) 
        {
            if (_initializationState != InitializationState.Uninitialized) return;

            _initializationState = InitializationState.Initializing;

            if (EventManager.Instance.GetListenerCount<InitializeLevelEvent>() > 0)
                EventManager.Instance.Publish(new InitializeLevelEvent());
            else
                OnInitializationComplete();
        }

        [EventListener]
        private void OnInitializationStopped(ExitStateEvent<LoadingState> e) 
        {
            if (_pendingInitializationCount <= 0)
                OnInitializationComplete();
        }

        [EventListener]
        private void EnterLevel(EnterStateEvent<GameplayState> e)
        {
            if (_initializationState != InitializationState.Initialized) return;

            EventManager.Instance.Publish(new EnterLevelEvent());
        }

        [EventListener]
        private void UpdateLevel(UpdateStateEvent<GameplayState> e)
        {
            EventManager.Instance.Publish(new UpdateLevelEvent());
        }

        [EventListener]
        private void ExitLevel(EnterGameExitEvent e)
        {
            if (_initializationState != InitializationState.Initialized) return;

            EventManager.Instance.Publish(new ExitLevelEvent());
            ResetState();

            var sceneFacade = SceneFacade.Instance;

            switch (e.Reason) 
            {
                case ExitGameReason.Restart:
                    sceneFacade.Reload();
                    break;

                case ExitGameReason.OpenNewLevel:
                    var targetScene = e.TargetScene;

                    if (targetScene.IsValid())
                        sceneFacade.Load(targetScene);
                    else
                        Debug.LogError(
                            $"LevelManager: the target scene is not defined while opening new level.");
                    break;

                case ExitGameReason.Exit:
                    if (sceneFacade.Load(mainSceneName)) return;

                    if (quitIfMissingMainScene)
                        ApplicationLibrary.QuitApp();
                    else
                        Debug.LogWarning($"LevelManager: Main scene '{mainSceneName}' is not found.");
                    break;

                default:
                    Debug.LogWarning(
                        $"LevelManager: A non-implemented reason '{e.Reason}' has been detected");
                    break;
            }
        }
        #endregion

        #region On Application Quit
        protected override void OnApplicationQuit()
        {
            if (EventManager.TryGetInstance(out var instance) && 
                _initializationState == InitializationState.Initialized)
                instance.Publish(new ExitLevelEvent());

            ResetState();
            base.OnApplicationQuit();
        }
        #endregion

        #region Private Methods
        private void OnInitializationComplete()
        {
            if (_initializationState != InitializationState.Initializing) return;

            _initializationState = InitializationState.Initialized;
            _pendingInitializationCount = 0;

            EventManager.Instance.Publish(new InitializeLevelCompleteEvent());

            if (GameManager.TryGetInstance(out var instance))
                instance.EnterGame();
            else
                Debug.LogWarning("Unable to enter game because game manager is not found");
        }

        private void ResetState() 
        {
            _initializationState = InitializationState.Uninitialized;
            _pendingInitializationCount = 0;
        }
        #endregion
    }
}