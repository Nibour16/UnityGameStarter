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

        public Timer CreateTimer(float duration)
        {
            var timer = new Timer(duration);
            _addBuffer.Add(timer);
            return timer;
        }

        public void RemoveTimer(Timer timer)
        {
            _removeBuffer.Add(timer);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            // Add a list of new timers
            if (_addBuffer.Count > 0)
            {
                _timers.AddRange(_addBuffer);
                _addBuffer.Clear();
            }

            // Update timers
            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Update(dt);

                if (_timers[i].TimerState == TimerState.Completed ||
                    _timers[i].TimerState == TimerState.Cancelled)
                    _removeBuffer.Add(_timers[i]);
            }

            // Remove timers
            if (_removeBuffer.Count > 0)
            {
                for (int i = 0; i < _removeBuffer.Count; i++)
                    _timers.Remove(_removeBuffer[i]);

                _removeBuffer.Clear();
            }
        }
    }
}