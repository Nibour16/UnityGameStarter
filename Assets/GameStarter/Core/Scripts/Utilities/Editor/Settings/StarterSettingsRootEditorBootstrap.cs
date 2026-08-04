using UnityEditor;
using UnityGameStarter.BuildCore;
using UnityGameStarter.StarterSettings.Config;

namespace UnityGameStarter.StarterSettings.Editor
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    static class StarterSettingsRootEditorBootstrap
    {
        static StarterSettingsRootEditorBootstrap()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                BakeStarterSettings();
        }

        private static void BakeStarterSettings()
        {
            StarterSettingsRootConfigGenerator.Sync();
        }

        [GameStarterBuild(GameStarterBuildPhase.Prebake, -500)]
        private static void BakeStarterSettingsForBuild()
        {
            StarterSettingsRootConfigGenerator.Sync();
        }
    }
    #endif
}
