using UnityEditor;

namespace UnityGameStarter.Config.Editor 
{
    using UnityGameStarter.AssetManagementLibrary;

    public static class ConfigEditorLibrary
    {
        public static void EnsureBootstrapFolder(string customFolderPath = "")
        {
            string folder =
                $"{ConfigLibrary.BootstrapRootFolder}/{ConfigLibrary.BootstrapConfigFolder}";

            if (!AssetDatabase.IsValidFolder(folder))
            {
                AssetManagementLibrary.EnsureFolder(ConfigLibrary.BootstrapRootFolder);

                AssetDatabase.CreateFolder(
                    ConfigLibrary.BootstrapRootFolder, ConfigLibrary.BootstrapConfigFolder);
            }

            if (!string.IsNullOrEmpty(customFolderPath))
            {
                AssetManagementLibrary.EnsureFolder(
                    $"{folder}/{customFolderPath}");
            }
        }
    }

}