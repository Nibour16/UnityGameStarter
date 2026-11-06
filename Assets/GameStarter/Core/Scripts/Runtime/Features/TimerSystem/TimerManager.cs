using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGameStarter.DataStructure;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.TimerSystem
{
    /// <summary>
    /// Manager that manages life cycles of all timers that are registered into the collection,
    /// including timer creation/registration methods
    /// </summary>
    [RuntimeSingleton]
    public class TimerManager : Singleton<TimerManager>
    {
        private readonly DeferredList<Timer> _timers = new();

        private readonly Dictionary<object, HashSet<Timer>> _owners = new();
        public Dictionary<object, HashSet<Timer>> TimerOwners => _owners;

        private readonly Dictionary<object, HashSet<Timer>> _tags = new();
        public Dictionary<object, HashSet<Timer>> TimerTags => _tags;

        #region API
        public Timer CreateTimer(object owner, float duration, string tag = "Default Timer", 
            TimerType timerType = TimerType.Default, bool autoStart = true) 
        {
            var timer = new Timer(owner, duration, tag, timerType);

            Register(timer);

            if (autoStart) timer.Start();
            return timer;
        }

        public void Register(Timer timer) => _timers.EnqueueAdd(timer);

        public void Unregister(Timer timer)
        {
            timer.OnRemoved();
            _timers.EnqueueRemove(timer);
        }

        public bool IsTimerStoped(Timer timer, bool includeIdle = false, bool includePaused = false)
        {
            return timer.TimerState switch
            {
                TimerState.Idle => includeIdle,
                TimerState.Paused => includePaused,
                TimerState.Completed => true,
                TimerState.Cancelled => true,
                _ => false
            };
        }
        #endregion

        #region Life Cycle
        private void Update()
        {
            UpdateTimers();

            ApplyTimerRemovals();
            ApplyTimerAdditions();
        }

        private void UpdateTimers() 
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Update(dt);

                if (CanRemoveTimer(_timers[i]))
                    Unregister(_timers[i]);
            }
        }

        private bool CanRemoveTimer(Timer timer) 
        {
            if (timer.TimerType != TimerType.AutoRemovable) return false;

            return IsTimerStoped(timer);
        }

        private void ApplyTimerAdditions()
        {
            foreach (var timer in _timers.PendingAdds) 
            {
                // Record to owner
                if (timer.Owner != null)
                {
                    if (!_owners.TryGetValue(timer.Owner, out var set))
                    {
                        set = new HashSet<Timer>();
                        _owners[timer.Owner] = set;
                    }

                    set.Add(timer);
                }

                // Record to tag
                if (timer.Tag != null)
                {
                    if (!_tags.TryGetValue(timer.Tag, out var set))
                    {
                        set = new HashSet<Timer>();
                        _tags[timer.Tag] = set;
                    }

                    set.Add(timer);
                }
            }
            
            _timers.ApplyAdditions();
        }

        private void ApplyTimerRemovals()
        {
            foreach (var timer in _timers.PendingRemoves) 
            {
                // Remove from Owner
                if (timer.Owner != null && _owners.TryGetValue(timer.Owner, out var set1))
                {
                    set1.Remove(timer);

                    if (set1.Count == 0)
                        _owners.Remove(timer.Owner);
                }

                // Remove from Tag
                if (timer.Tag != null && _tags.TryGetValue(timer.Tag, out var set2))
                {
                    set2.Remove(timer);

                    if (set2.Count == 0)
                        _tags.Remove(timer.Tag);
                }
            }

            _timers.ApplyRemovals();
        }
        #endregion
    }
}