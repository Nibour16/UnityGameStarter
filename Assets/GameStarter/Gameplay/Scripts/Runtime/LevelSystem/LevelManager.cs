using UnityEngine;
using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.LevelManagement 
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private string mainSceneName;

        private bool _initialized = false;
        public bool Initialized => _initialized;

        public void InitializeLevel() 
        {
            if (_initialized) return;

            EventManager.Instance.Publish(new InitializeLevelEvent());
        }

        public void OnInitializationComplete() 
        {
            if (_initialized) return;

            _initialized = true;
            EventManager.Instance.Publish(new InitializeLevelCompleteEvent());
        }
        
        public void EnterLevel()
        {
            if (!_initialized)
            {
                Debug.LogWarning("Cannot enter level before initialization.");
                return;
            }

            EventManager.Instance.Publish(new EnterLevelEvent());
        }

        public void UpdateLevel()
        {
            EventManager.Instance.Publish(new UpdateLevelEvent());
        }

        public void ExitLevel()
        {
            if (!_initialized)
            {
                Debug.LogWarning("Cannot exit level before initialization.");
                return;
            }

            EventManager.Instance.Publish(new ExitLevelEvent());
            _initialized = false;

            var sceneFacade = SceneFacade.Instance;
            sceneFacade.Load(sceneFacade.GetSceneByName(mainSceneName));
        }

        protected override void OnApplicationQuit()
        {
            if (EventManager.HasInstance && _initialized)
                EventManager.Instance.Publish(new ExitLevelEvent());
            
            _initialized = false;

            base.OnApplicationQuit();
        }
    }
}