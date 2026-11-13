using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.UnityObjectStatics 
{
    public static class ObjectLibrary
    {
        public static bool IsValid(this object obj) 
        {
            if (obj is Object unityObj) return unityObj != null;

            return obj != null;
        }

        public static bool IsDestroyed(this Object obj) => !ReferenceEquals(obj, null) && obj == null;

        public static IEnumerable<T> FindObjectsInCurrentStage<T>(bool includeInactive = false) where T : Component
        {
            #if UNITY_EDITOR
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

            // Prefab Mode
            if (prefabStage != null)
                return prefabStage.prefabContentsRoot.GetComponentsInChildren<T>(includeInactive);
            #endif

            // Scene
            return Object.FindObjectsByType<T>(
                includeInactive? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
        }

        public static bool HasCircularReference<T>(
            this T current, T target, System.Func<T, IEnumerable<T>> getChildren, HashSet<T> visited = null)
        {
            if (EqualityComparer<T>.Default.Equals(current, target)) return true;

            visited ??= new HashSet<T>();

            if (!visited.Add(current)) return false;

            foreach (var child in getChildren(current))
            {
                if (HasCircularReference(child, target, getChildren, visited))
                    return true;
            }

            return false;
        }
    }
}