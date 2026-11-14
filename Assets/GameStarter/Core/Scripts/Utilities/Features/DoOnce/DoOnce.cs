using System;
using System.Collections.Generic;

namespace UnityGameStarter.DoOnceStatics 
{
    /// <summary>
    /// Class with functionalities for handling actions that requires to be executed once
    /// </summary>
    public static class DoOnce
    {
        private static readonly HashSet<string> _executed = new();

        /// <summary>
        /// Execute the targe Do Once method, the target method won't be executed if it is already executed
        /// </summary>
        public static bool Run(string id, Action action)
        {
            if (!_executed.Add(id)) return false;

            action();
            return true;
        }

        /// <summary>
        /// Was the target Do Once method executed
        /// </summary>
        public static bool IsDone(string id) => _executed.Contains(id);

        /// <summary>
        /// Reset the target Do Once method so that the target method can be executed again
        /// </summary>
        public static void Reset(string id) => _executed.Remove(id);

        /// <summary>
        /// Reset all Do Once methods, no matter what about who they are
        /// </summary>
        public static void ResetAll() => _executed.Clear();
    }
}