using System;

namespace UnityGameStarter.TimerSystem 
{
    public enum TimerState
    {
        Idle,
        Running,
        Paused,
        Completed,
        Cancelled
    }

    public class Timer 
    {
#nullable enable

        #region Identity
        /// <summary> Entity that owns the timer </summary>
        private readonly object? _owner;
        public object? Owner => _owner;

        /// <summary> Tag that indetifies the timer </summary>
        private readonly string? _tag;
        public string? Tag => _tag;
        #endregion

        #region Time in sec
        /// <summary> Total time </summary>
        private readonly float _duration;
        public float Duration => _duration;

        /// <summary> Running time </summary>
        private float _elapsedTime;
        public float ElapsedTime => _elapsedTime;

        /// <summary> Remaining time </summary>
        public float RemainingTime => MathF.Max(0f, _duration - _elapsedTime);
        #endregion

        #region Timer States
        private TimerState _timerState;
        public TimerState TimerState => _timerState;

        private readonly bool _autoRemove;
        public bool AutoRemove => _autoRemove;
        #endregion

        #region Progress
        public float Progress => _duration <= 0f ? 1f : _elapsedTime / _duration;

        public event Action? Completed;
        #endregion

        #region Initialization
        public Timer(object owner, float duration, string tag = "Default Timer", bool autoRemove = true)
        {
            _owner = owner;
            _tag = tag;
            _autoRemove = autoRemove;

            _duration = MathF.Max(0f, duration);
            _timerState = TimerState.Idle;
        }
        #endregion

        #region Life Cycle
        public void Reset()
        {
            _elapsedTime = 0f;
        }

        public void Start(bool reset = true)
        {
            if (reset) Reset();

            _timerState = TimerState.Running;
        }

        public void Update(float deltaTime)
        {
            if (_timerState != TimerState.Running) return;

            _elapsedTime += deltaTime;

            if (_elapsedTime >= _duration)
            {
                _elapsedTime = _duration;
                _timerState = TimerState.Completed;

                Completed?.Invoke();
            }
        }

        public void Pause()
        {
            if (_timerState == TimerState.Running) _timerState = TimerState.Paused;
        }

        public void Resume()
        {
            if (_timerState == TimerState.Paused) _timerState = TimerState.Running;
        }

        public void Cancel()
        {
            _timerState = TimerState.Cancelled;
        }

        public void Restart(bool keepState = true)
        {
            Reset();

            if (!keepState) Start(false);
        }
        #endregion
    }
}