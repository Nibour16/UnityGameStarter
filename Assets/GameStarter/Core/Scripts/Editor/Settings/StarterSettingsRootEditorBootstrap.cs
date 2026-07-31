using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameStarter.Config;
using UnityGameStarter.ProjectSettings;
using UnityGameStarter.StarterSettings.Config;

namespace UnityGameStarter.StarterSettings.Editor
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    static class StarterSettingsRootEditorBootstrap
    {
        static StarterSettingsRootEditorBootstrap()
        {
            if (!File.Exists(ConfigLibrary.GetBootstrapAssetPath("StarterSettingsRootConfig")))
                BakeStarterSettings();
        }

        private static void BakeStarterSettings()
        {
            var root = ProjectSettingReference<StarterSettingsRoot>.Get("UnityGameStarter.SettingsRoot");

            if (!root) return;

            var config =
                ScriptableObject.CreateInstance<StarterSettingsRootConfig>();

            config.SetConfig(root);

            AssetDatabase.CreateAsset(
                config, ConfigLibrary.GetBootstrapAssetPath("StarterSettingsRootConfig"));
        }
    }
    #endif
}
