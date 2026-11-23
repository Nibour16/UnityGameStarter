using UnityEngine;
using UnityGameStarter.Config;
using UnityGameStarter.RuntimeCore;
using UnityGameStarter.StarterSettings.Config;

namespace UnityGameStarter.StarterSettings.Runtime
{
    static class StarterSettingsRuntimeRootBootstrap
    {
        [GameStarterRuntime(RuntimeInitializeLoadType.BeforeSceneLoad, -500)]
        private static void RuntimeInitialize()
        {
            var root = GetRoot();

            if (!root) return;
            
            StarterSettingsProvider.Initialize(root);
        }

        private static StarterSettingsRoot GetRoot() 
        {
            var config = Resources.Load<StarterSettingsRootConfig>(
                            ConfigLibrary.GetBootstrapResourcePath("StarterSettingsRootConfig"));

            if (!config) 
            {
                Debug.LogError("Runtime Bootstrap: Failed to load Starter Settings Root Config");
                return null;
            }

            return config.Config;
        }
    }
}