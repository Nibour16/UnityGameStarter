using UnityEngine;
using UnityGameStarter.ProjectSettings;
using UnityGameStarter.RuntimeCore;

namespace UnityGameStarter.StarterSettings 
{
    public static class StarterSettingsRootBootstrap
    {
        [GameStarter(RuntimeInitializeLoadType.BeforeSceneLoad, -500)]
        public static void Initialize()
        {
            StarterSettingsProvider.Initialize(FindSettingsRoot());
        }

        private static StarterSettingsRoot FindSettingsRoot()
        {
            var root = ProjectSettingReference<StarterSettingsRoot>.Get(
                "UnityGameStarter.SettingsRoot");

            if (root == null)
            {
                Debug.LogError("UnityGameStarterSettingsRoot not found.");
                return null;
            }
            return root;
        }
    }
}