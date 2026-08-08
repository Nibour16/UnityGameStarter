using UnityEngine;
using UnityGameStarter.CommonData;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.PauseManagement 
{
    public class PauseChangedEvent
    {
        public bool IsPaused { get; }

        public PauseChangedEvent(bool paused)
        {
            IsPaused = paused;
        }
    }

    [RuntimeSingleton(-100)]
    public class PauseManager : Singleton<PauseManager>, IAutoEventListener
    {
        private bool _isPaused = false;
        private float _timeScale = 1f;

        protected override void Awake()
        {
            base.Awake();

            if (!TryGetComponent<EventListenerRegister>(out _))
                gameObject.AddComponent<EventListenerRegister>();
        }

        public void SetPause(bool pause)
        {
            if (_isPaused == pause)
                return;

            _isPaused = pause;

            Time.timeScale = _isPaused ? 0f : _timeScale;

            EventManager.Instance.Publish(
                new PauseChangedEvent(_isPaused));
        }

        [EventListener]
        private void SetGameTimeScale(RuntimeScale time)
        {
            _timeScale = Mathf.Max(0f, time.TimeScale);

            if (!_isPaused)
                Time.timeScale = _timeScale;
        }
    }
}