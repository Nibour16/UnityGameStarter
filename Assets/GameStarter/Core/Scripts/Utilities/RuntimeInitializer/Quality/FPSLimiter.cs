using UnityEngine;
using UnityGameStarter.RuntimeCore;

namespace UnityGameStarter.StarterSettings.Quality 
{
    public static class FPSLimiter
    {
        [GameStarter(RuntimeInitializeLoadType.BeforeSceneLoad, -490)]
        public static void Initialize()
        {
            var settings = StarterSettingsProvider.Get<FPSSettings>();

            if (settings == null)
                return;

            if (settings.EnableVSync)
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
            }
            else 
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = settings.TargetFrameRate;
            }
        }
    }
}