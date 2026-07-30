using UnityGameStarter.ProjectSettings;

namespace UnityGameStarter.StarterSettings 
{
    public static class StarterSettingsRootProvider
    {
        public static StarterSettingsRoot GetRoot()
        {
            return ProjectSettingReference<StarterSettingsRoot>.Get(
                "UnityGameStarter.SettingsRoot");
        }
    }
}