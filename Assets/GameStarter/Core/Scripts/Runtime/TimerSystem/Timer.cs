using System;
using static UnityEditorInternal.ReorderableList;

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

    public enum TimerType 
    {
        Default,
        AutoRemovable,
        Loopable
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

        /// <summary> Should remove the timer automatically when completed/cancelled </summary>
        private readonly TimerType _timerType;
        public TimerType TimerType => _timerType;
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
        #endregion

        #region Progress
        public float Progress => _duration <= 0f ? 1f : _elapsedTime / _duration;

        private event Action? Completed;
        private event Action? Cancelled;
        private event Action? Removed;
        #endregion

        #region Initialization
        public Timer(
            object owner, float duration, string tag = "Default Timer", TimerType timerType = TimerType.Default)
        {
            _owner = owner;
            _tag = tag;
            _timerType = timerType;

            _duration = MathF.Max(0f, duration);
            _timerState = TimerState.Idle;
        }
        #endregion

        #region Event Life Cycle
        public void BindCompleted(Action completed) => Completed += completed;
        public void UnbindCompleted(Action completed) => Completed -= completed;
        public void UnbindCompletedAll() => Completed = null;

        public void BindCancelled(Action cancelled) => Cancelled += cancelled;
        public void UnbindCancelled(Action cancelled) => Cancelled -= cancelled;
        public void UnbindCancelledAll() => Cancelled = null;

        public void BindRemoved(Action removed) => Removed += removed;
        public void UnbindRemoved(Action removed) => Removed -= removed;
        public void UnbindRemovedAll() => Removed = null;
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

        public void Start(Action completed, bool reset = true) 
        {
            BindCompleted(completed);
            Start(reset);
        }

        public void Start(Action completed, Action cancelled, bool reset = true)
        {
            BindCancelled(cancelled);
            Start(completed, reset);
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

                if (_timerType == TimerType.Loopable)
                    Restart();
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
            Cancelled?.Invoke();
        }

        public void Restart(bool keepState = true)
        {
            Reset();

            if (!keepState || _timerState == TimerState.Completed || _timerState == TimerState.Cancelled) 
                Start(false);
        }

        public void OnRemoved()
        {
            Removed?.Invoke();
        }
        #endregion
    }
}