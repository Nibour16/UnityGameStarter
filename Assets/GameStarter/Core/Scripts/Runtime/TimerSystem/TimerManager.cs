using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.TimerSystem 
{
    public class TimerManager : Singleton<TimerManager>
    {
        private readonly List<Timer> _timers = new();

        private readonly List<Timer> _addBuffer = new();
        private readonly List<Timer> _removeBuffer = new();

        #region API
        public Timer EnqueueCreateTimer(
            object owner, float duration, string tag = "Default Timer", bool autoRemove = true)
        {
            var timer = new Timer(owner, duration, tag, autoRemove);
            _addBuffer.Add(timer);
            return timer;
        }

        public void EnqueueRemoveTimer(Timer timer)
        {
            _removeBuffer.Add(timer);
        }
        #endregion

        #region Update
        private void Update()
        {
            ProcessPendingCreates();
            UpdateTimers();
            ProcessPendingRemovals();
        }

        private void UpdateTimers()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Update(dt);

                if ((_timers[i].TimerState == TimerState.Completed || _timers[i].TimerState == TimerState.Cancelled)
                    && _timers[i].AutoRemove)
                    _removeBuffer.Add(_timers[i]);
            }
        }
        #endregion

        #region Process Pending items
        private void ProcessPendingCreates() 
        {
            if (_addBuffer.Count > 0)
            {
                _timers.AddRange(_addBuffer);
                _addBuffer.Clear();
            }
        }

        private void ProcessPendingRemovals() 
        {
            if (_removeBuffer.Count > 0)
            {
                for (int i = 0; i < _removeBuffer.Count; i++)
                    _timers.Remove(_removeBuffer[i]);

                _removeBuffer.Clear();
            }
        }
        #endregion
    }
}