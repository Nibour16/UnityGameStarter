using UnityEditor;
using UnityGameStarter.ProjectSettings;

namespace UnityGameStarter.StarterSettings.Editor
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
