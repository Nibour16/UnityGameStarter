using UnityEditor;
using UnityGameStarter.EditorUtilities.ProjectSettings;
using UnityGameStarter.StarterSettings;

namespace UnityGameStarter.EditorUtilities.StarterSettings
{
    #if UNITY_EDITOR
    public static class StarterSettingsRootEditor
    {
        private const string Key = "UnityGameStarter.SettingsRoot";

        [SettingsProvider]
        public static SettingsProvider Create()
        {
            return ProjectSettingReferenceEditor<StarterSettingsRoot>.Create(
                "Project/Unity Game Starter", "Settings Root", Key);
        }
    }
    #endif
}
