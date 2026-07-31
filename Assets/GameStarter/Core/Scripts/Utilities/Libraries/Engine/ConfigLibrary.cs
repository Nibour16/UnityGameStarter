namespace UnityGameStarter.Config 
{
    public static class ConfigLibrary
    {
        public const string BootstrapConfigFolder = "BootstrapConfigs";
        public const string BootstrapRootFolder = "Assets/Resources";

        public static string GetBootstrapAssetPath(string configName, string customFolderPath = "")
        {
            string folder = $"Assets/Resources/{BootstrapConfigFolder}";

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