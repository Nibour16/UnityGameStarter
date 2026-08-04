using UnityEditor;
using UnityEngine;
using UnityGameStarter.Config;
using UnityGameStarter.Config.Editor;
using UnityGameStarter.ProjectSettings;

namespace UnityGameStarter.StarterSettings.Config
{
    public static class StarterSettingsRootConfigGenerator
    {
        public static void Sync()
        {
            var root = ProjectSettingReference<StarterSettingsRoot>.Get("UnityGameStarter.SettingsRoot");

            if (!root) return;

            ConfigEditorLibrary.EnsureBootstrapFolder();

            string path = ConfigLibrary.GetBootstrapAssetPath("StarterSettingsRootConfig");

            var config = AssetDatabase.LoadAssetAtPath<StarterSettingsRootConfig>(path);

            if (config == null)
            {
                config = ScriptableObject.CreateInstance<StarterSettingsRootConfig>();

                config.SetConfig(root);

                AssetDatabase.CreateAsset(config, path);
            }
            else
            {
                config.SetConfig(root);

                EditorUtility.SetDirty(config);
            }

            AssetDatabase.SaveAssets();
        }
    }
}