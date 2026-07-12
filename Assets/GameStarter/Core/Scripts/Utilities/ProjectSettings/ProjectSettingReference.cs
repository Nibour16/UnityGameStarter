using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.ProjectSettings 
{
    public static class ProjectSettingReference<T> where T : ScriptableObject
    {
        private static T _target;

        public static T Get(string key)
        {
            if (_target != null) return _target;

            string guid = EditorPrefs.GetString(key, string.Empty);

            if (string.IsNullOrEmpty(guid)) return null;

            string path = AssetDatabase.GUIDToAssetPath(guid);

            _target = AssetDatabase.LoadAssetAtPath<T>(path);

            return _target;
        }

        public static void Set(string key, T value)
        {
            _target = value;

            if (_target == null)
            {
                EditorPrefs.DeleteKey(key);
                return;
            }

            string path = AssetDatabase.GetAssetPath(_target);
            string guid = AssetDatabase.AssetPathToGUID(path);
            EditorPrefs.SetString(key, guid);
        }
    }
}

