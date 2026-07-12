using UnityEngine;
using UnityGameStarter.StarterSettings;

namespace UnityGameStarter.RuntimeCore 
{
    public static class UnityGameStarterBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var root = FindSettingsRoot();

            if (root == null)
            {
                Debug.LogError("UnityGameStarterSettingsRoot not found.");
                return;
            }

            StarterSettingsProvider.Initialize(root);
        }

        private static StarterSettingsRoot FindSettingsRoot()
        {
            // TODO
            return null;
        }
    }
}