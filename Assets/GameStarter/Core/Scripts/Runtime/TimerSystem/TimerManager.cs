using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.TimerSystem 
{
    public class TimerManager : Singleton<TimerManager>
    {
        private readonly List<Timer> _timers = new();

        private readonly Dictionary<object, HashSet<Timer>> _ownerIndex = new();
        public Dictionary<object, HashSet<Timer>> OwnerIndex => _ownerIndex;

        private readonly Dictionary<object, HashSet<Timer>> _tagIndex = new();
        public Dictionary<object, HashSet<Timer>> TagIndex => _tagIndex;

        #region API
        public void Register(Timer timer)
        {
            _timers.Add(timer);

            // Owner index
            if (timer.Owner != null)
            {
                if (!_ownerIndex.TryGetValue(timer.Owner, out var set))
                {
                    set = new HashSet<Timer>();
                    _ownerIndex[timer.Owner] = set;
                }

                set.Add(timer);
            }

            // Tag index
            if (timer.Tag != null)
            {
                if (!_tagIndex.TryGetValue(timer.Tag, out var set))
                {
                    set = new HashSet<Timer>();
                    _tagIndex[timer.Tag] = set;
                }

                set.Add(timer);
            }
        }

        public void Unregister(Timer timer)
        {
            _timers.Remove(timer);

            if (timer.Owner != null &&
                _ownerIndex.TryGetValue(timer.Owner, out var set1))
            {
                set1.Remove(timer);
                if (set1.Count == 0)
                    _ownerIndex.Remove(timer.Owner);
            }

            if (timer.Tag != null &&
                _tagIndex.TryGetValue(timer.Tag, out var set2))
            {
                set2.Remove(timer);
                if (set2.Count == 0)
                    _tagIndex.Remove(timer.Tag);
            }
        }
        #endregion

        #region Update
        private void Update()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Update(dt);

                if ((_timers[i].TimerState == TimerState.Completed || _timers[i].TimerState == TimerState.Cancelled)
                    && _timers[i].AutoRemove)
                    Unregister(_timers[i]);
            }
        }
        #endregion
    }
}