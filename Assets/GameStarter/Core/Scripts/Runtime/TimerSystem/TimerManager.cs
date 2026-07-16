using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.DefferedDataStructure;

namespace UnityGameStarter.TimerSystem
{
    [RuntimeSingleton]
    public class TimerManager : Singleton<TimerManager>
    {
        private readonly DefferedList<Timer> _timers = new();

        private readonly Dictionary<object, HashSet<Timer>> _ownerIndex = new();
        public Dictionary<object, HashSet<Timer>> OwnerIndex => _ownerIndex;

        private readonly Dictionary<object, HashSet<Timer>> _tagIndex = new();
        public Dictionary<object, HashSet<Timer>> TagIndex => _tagIndex;

        #region API
        public Timer CreateTimer(
            object owner, float duration, string tag = "Default Timer", 
            TimerType timerType = TimerType.Default, bool autoStart = true) 
        {
            var timer = new Timer(owner, duration, tag, timerType);

            Register(timer);

            if (autoStart) timer.Start();
            return timer;
        }

        public void Register(Timer timer)
        {
            _timers.EnqueueAdd(timer);
        }

        public void Unregister(Timer timer)
        {
            timer.OnRemoved();
            _timers.EnqueueRemove(timer);
        }
        #endregion

        #region Life Cycle
        private void Update()
        {
            UpdateTimers();

            OnTimersRemoved();
            _timers.ApplyRemovals();

            OnTimersAdded();
            _timers.ApplyAdditions();
        }

        private void UpdateTimers() 
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Update(dt);

                if ((_timers[i].TimerState == TimerState.Completed || _timers[i].TimerState == TimerState.Cancelled)
                    && _timers[i].TimerType == TimerType.AutoRemovable)
                    Unregister(_timers[i]);
            }
        }

        private void OnTimersAdded()
        {
            foreach (var t in _timers.PendingAdds)
            {
                AddToIndex(t);
            }
        }

        private void OnTimersRemoved()
        {
            foreach (var t in _timers.PendingRemoves)
            {
                RemoveFromIndex(t);
            }
        }
        #endregion

        #region Index Collection
        private void AddToIndex(Timer t)
        {
            if (t.Owner != null)
            {
                if (!_ownerIndex.TryGetValue(t.Owner, out var set))
                {
                    set = new HashSet<Timer>();
                    _ownerIndex[t.Owner] = set;
                }
                set.Add(t);
            }

            if (t.Tag != null)
            {
                if (!_tagIndex.TryGetValue(t.Tag, out var set))
                {
                    set = new HashSet<Timer>();
                    _tagIndex[t.Tag] = set;
                }
                set.Add(t);
            }
        }

        private void RemoveFromIndex(Timer t)
        {
            if (t.Owner != null &&
                _ownerIndex.TryGetValue(t.Owner, out var set1))
            {
                set1.Remove(t);
                if (set1.Count == 0)
                    _ownerIndex.Remove(t.Owner);
            }

            if (t.Tag != null &&
                _tagIndex.TryGetValue(t.Tag, out var set2))
            {
                set2.Remove(t);
                if (set2.Count == 0)
                    _tagIndex.Remove(t.Tag);
            }
        }
        #endregion
    }
}