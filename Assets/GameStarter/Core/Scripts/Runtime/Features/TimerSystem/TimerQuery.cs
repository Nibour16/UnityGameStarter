using System.Collections.Generic;
using System.Linq;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.TimerSystem.Query
{
    public class TimerQuery : Singleton<TimerQuery>
    {
        public IEnumerable<Timer> ByOwner(object owner)
            => TimerManager.Instance.OwnerIndex.TryGetValue(owner, out var set) ? set : Enumerable.Empty<Timer>();

        public IEnumerable<Timer> ByTag(object tag)
            => TimerManager.Instance.TagIndex.TryGetValue(tag, out var set) ? set : Enumerable.Empty<Timer>();

        public bool HasOwner(object owner) => TimerManager.Instance.OwnerIndex.ContainsKey(owner);

        public bool HasTag(object tag) => TimerManager.Instance.TagIndex.ContainsKey(tag);

        public int CountByOwner(object owner)
            => TimerManager.Instance.OwnerIndex.TryGetValue(owner, out var set) ? set.Count : 0;

        public int CountByTag(object tag)
            => TimerManager.Instance.TagIndex.TryGetValue(tag, out var set) ? set.Count : 0;
    }
}