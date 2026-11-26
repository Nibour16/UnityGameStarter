using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.EditorUtilities.Installation
{
    [InitializeOnLoad]
    static class TemplateInstaller
    {
        static TemplateInstaller()
        {
            PackageInitializer.RequestInstall(Install);
        }

        private static void Install(UnityEditor.PackageManager.PackageInfo info) 
        {
            if (info == null) return;

            string projectPath = Directory.GetParent(Application.dataPath).FullName;
            string targetPath = Path.Combine(projectPath, PackageSynchronizer.starterTemplatesSource);

            if (Directory.Exists(targetPath)) return;

            string sourcePath = Path.Combine(info.resolvedPath, PackageSynchronizer.starterTemplatesPackage);

            if (!Directory.Exists(sourcePath))
            {
                Debug.LogError("Templates directory not found.");
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            FileUtil.CopyFileOrDirectory(sourcePath, targetPath);
            AssetDatabase.Refresh();

            Debug.Log($"Unity Game Starter Templates installed successfully at: {targetPath}");
        }
    }
}
