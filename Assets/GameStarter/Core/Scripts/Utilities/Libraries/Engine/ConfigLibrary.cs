namespace UnityGameStarter.Config 
{
    public static class ConfigLibrary
    {
        private const string BootstrapConfigFolder = "BootstrapConfigs";

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