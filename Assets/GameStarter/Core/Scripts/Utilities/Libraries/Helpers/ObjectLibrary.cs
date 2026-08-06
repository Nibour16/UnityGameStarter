using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.UnityObjectStatics 
{
    public static class ObjectLibrary
    {
        public static bool IsValid(this object obj) 
        {
            if (obj is Object unityObj)
                return unityObj != null;

            return obj != null;
        }

        public static bool IsDestroyed(this Object obj)
            => !ReferenceEquals(obj, null) && obj == null;

        public static IEnumerable<T> FindObjectsInCurrentStage<T>() where T : Component
        {
            #if UNITY_EDITOR
            var prefabStage = UnityEditor.SceneManagement
                .PrefabStageUtility.GetCurrentPrefabStage();

            // Prefab Mode
            if (prefabStage != null)
            {
                return prefabStage.prefabContentsRoot
                    .GetComponentsInChildren<T>(true);
            }
            #endif

            // Scene
            return Object.FindObjectsByType<T>(FindObjectsInactive.Include);
        }
    }
}