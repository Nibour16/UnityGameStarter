using UnityEngine;
using UnityGameStarter.RuntimeCore;

namespace UnityGameStarter.StarterSettings
{
    public static class StarterSettingsRootBootstrap
    {
        private static StarterSettingsRoot _root;

        [GameStarterRuntime(RuntimeInitializeLoadType.BeforeSceneLoad, -500)]
        private static void RuntimeInitialize()
        {
            if (_root == null)
                _root = StarterSettingsRootProvider.GetRoot();

            StarterSettingsProvider.Initialize(_root);
        }
    }
}