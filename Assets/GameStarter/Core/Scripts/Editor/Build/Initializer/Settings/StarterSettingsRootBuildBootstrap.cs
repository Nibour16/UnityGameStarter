using UnityEditor;
using UnityEngine;
using UnityGameStarter.BuildCore;
using UnityGameStarter.Config;
using UnityGameStarter.ProjectSettings;
using UnityGameStarter.StarterSettings.Config;

namespace UnityGameStarter.StarterSettings.Build
{
    static class StarterSettingsRootBuildBootstrap
    {
        [GameStarterBuild(GameStarterBuildPhase.Prebake, -500)]
        private static void BakeStarterSettings()
        {
            var root = ProjectSettingReference<StarterSettingsRoot>.Get("UnityGameStarter.SettingsRoot");

            var config =
                ScriptableObject.CreateInstance<StarterSettingsRootConfig>();

            config.SetConfig(root);

            AssetDatabase.CreateAsset(
                config, ConfigLibrary.GetBootstrapAssetPath("StarterSettingsRootConfig"));
        }
    }
}

