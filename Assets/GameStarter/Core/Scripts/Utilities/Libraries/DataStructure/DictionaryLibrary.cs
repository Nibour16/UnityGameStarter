using System;
using System.Collections.Generic;

namespace UnityGameStarter.DataStructure 
{
    public static class DictionaryLibrary
    {
        public static void AddRangeSafe<TKey, TValue>(
            this IDictionary<TKey, TValue> target, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var kvp in items)
            {
                if (!target.ContainsKey(kvp.Key))
                {
                    target.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}