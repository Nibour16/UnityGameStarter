using System.IO;
using UnityEditor;

namespace UnityGameStarter.EditorUtilities.AssetManagement
{
    public static class AssetManagementLibrary
    {
        public static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
                return;

            string parent = Path.GetDirectoryName(folderPath);
            string folderName = Path.GetFileName(folderPath);

            if (!AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}