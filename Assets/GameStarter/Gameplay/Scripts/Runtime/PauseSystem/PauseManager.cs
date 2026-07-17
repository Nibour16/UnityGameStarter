using UnityEngine;
using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.Gameplay.Core;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.PauseManagement 
{
    public class PauseChangedEvent
    {
        public bool IsPaused { get; }

        public PauseChangedEvent(bool paused)
        {
            IsPaused = paused;
        }
    }

    public class PauseManager : Singleton<PauseManager>
    {
        private bool _isPaused = false;

        public void SetPause(bool pause)
        {
            if (_isPaused == pause) return;

            _isPaused = pause;

            var gameplayData = GameManager.Instance.data;
            float gameTimeScale = gameplayData ? gameplayData.gameTimeScale : 1f;

            Time.timeScale = _isPaused ? 0f : gameTimeScale;

            EventManager.Instance.Publish(new PauseChangedEvent(_isPaused));
        }
    }
}