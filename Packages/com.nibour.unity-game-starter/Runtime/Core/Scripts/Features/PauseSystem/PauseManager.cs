using UnityEngine;
using UnityGameStarter.CommonData;
using UnityGameStarter.Events;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.PauseManagement 
{
    /// <summary>
    /// Offers pause change event when 
    /// </summary>
    public class PauseChangedEvent
    {
        public bool IsPaused { get; }

        public PauseChangedEvent(bool paused)
        {
            IsPaused = paused;
        }
    }

    /// <summary>
    /// A manager that handles runtime pause, publish events to related logic during pause change events
    /// </summary>
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

        /// <summary>
        /// Set pause state and then publish the pause change event
        /// </summary>
        public void SetPause(bool pause)
        {
            if (_isPaused == pause)
                return;

            _isPaused = pause;

            Time.timeScale = _isPaused ? 0f : _timeScale;

            EventManager.Instance.Publish(
                new PauseChangedEvent(_isPaused));
        }

        /// <summary>
        /// Define game time scale for unpause runtime. The reference value is from the logic that publishes it
        /// It cannot rely any gameplay functionalities in the Game Starter or other related games 
        /// because it is a core manager for runtime pause system.
        /// Therefore the purpose is mainly for decoupling the dependencies
        /// </summary>
        [EventListener]
        private void SetGameTimeScale(RuntimeScale time)
        {
            _timeScale = Mathf.Max(0f, time.TimeScale);

            if (!_isPaused)
                Time.timeScale = _timeScale;
        }
    }
}