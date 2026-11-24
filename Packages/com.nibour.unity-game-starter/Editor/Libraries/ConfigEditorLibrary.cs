using UnityEditor;

namespace UnityGameStarter.EditorUtilities.Config 
{
    using UnityGameStarter.Config;
    using UnityGameStarter.EditorUtilities.AssetManagement;

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