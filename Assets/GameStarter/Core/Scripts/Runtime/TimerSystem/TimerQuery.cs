using System;
using System.Collections.Generic;
using System.Linq;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.TimerSystem 
{
    public class TimerQuery : Singleton<TimerQuery>
    {
        public IEnumerable<Timer> ByOwner(object owner)
        {
            return TimerManager.Instance.Timers.Where(t => Equals(t.Owner, owner));
        }

        public IEnumerable<Timer> ByTag(object tag)
        {
            return TimerManager.Instance.Timers.Where(t => Equals(t.Tag, tag));
        }

        public IEnumerable<Timer> Where(Predicate<Timer> predicate)
        {
            return TimerManager.Instance.Timers.Where(t => predicate(t));
        }
    }
}