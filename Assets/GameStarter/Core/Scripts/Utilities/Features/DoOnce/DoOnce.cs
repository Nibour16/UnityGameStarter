using System;
using System.Collections.Generic;

namespace UnityGameStarter.DoOnceStatics 
{
    public static class DoOnce
    {
        private static readonly HashSet<string> _executed = new();

        public static bool Run(string id, Action action)
        {
            if (!_executed.Add(id)) return false;

            action();
            return true;
        }

        public static bool IsDone(string id) => _executed.Contains(id);
        public static void Reset(string id) => _executed.Remove(id);
        public static void ResetAll() => _executed.Clear();
    }
}