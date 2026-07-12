#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.ProjectSettings
{
    public static class ProjectSettingReferenceEditor<T> where T : ScriptableObject
    {
        public static SettingsProvider Create(string path, string label, string key)
        {
            return new SettingsProvider(path, SettingsScope.Project)
            {
                guiHandler = _ =>
                {
                    var current = ProjectSettingReference<T>.Get(key);

                    var result = (T)EditorGUILayout.ObjectField(label, current, typeof(T), false);

                    if (result != current)
                        ProjectSettingReference<T>.Set(key, result);
                }
            };
        }
    }
}
#endif