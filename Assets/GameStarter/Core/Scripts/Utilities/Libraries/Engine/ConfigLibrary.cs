namespace UnityGameStarter.Config 
{
    public static class ConfigLibrary
    {
        public const string BootstrapConfigFolder = "BootstrapConfigs";
        public const string BootstrapRootFolder = "Assets/GameStarter/Resources";

        public static string GetBootstrapAssetPath(string configName, string customFolderPath = "")
        {
            string folder = $"{BootstrapRootFolder}/{BootstrapConfigFolder}";

            if (!string.IsNullOrEmpty(customFolderPath))
                folder += "/" + customFolderPath;

            return $"{folder}/{configName}.asset";
        }

        public static string GetBootstrapResourcePath(string configName, string customFolderPath = "")
        {
            string folder = BootstrapConfigFolder;

            if (!string.IsNullOrEmpty(customFolderPath))
                folder += "/" + customFolderPath;

            return $"{folder}/{configName}";
        }
    }
}