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
            var config = Resources.Load<StarterSettingsRootConfig>(
                ConfigLibrary.GetBootstrapResourcePath("StarterSettingsRootConfig"));

            StarterSettingsProvider.Initialize(config.Config);
        }
    }
}