using System.Collections.Generic;
using System.Linq;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.TimerSystem 
{
    public class TimerQuery : Singleton<TimerQuery>
    {
        public IEnumerable<Timer> ByOwner(object owner)
        {
            return TimerManager.Instance.OwnerIndex.TryGetValue(owner, out var set)
                ? set : Enumerable.Empty<Timer>();
        }

        public IEnumerable<Timer> ByTag(object tag)
        {
            return TimerManager.Instance.TagIndex.TryGetValue(tag, out var set)
                ? set : Enumerable.Empty<Timer>();
        }
    }
}