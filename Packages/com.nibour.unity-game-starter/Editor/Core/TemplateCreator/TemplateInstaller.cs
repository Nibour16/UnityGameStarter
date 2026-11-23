using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.TemplateInstallation
{
    [InitializeOnLoad]
    public static class TemplateInstaller
    {
        private const string sourceFolder = "Templates~";
        private const string targetFolder = "GameStarterTemplates";

        static TemplateInstaller()
        {
            EditorApplication.delayCall += Install;
        }

        private static void Install() 
        {
            string targetPath = Path.Combine(Application.dataPath, targetFolder);
            if (Directory.Exists(targetPath)) return;

            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(
                typeof(TemplateInstaller).Assembly);

            if (packageInfo == null)
            {
                Debug.LogError("Unable to locate Unity Game Starter package.");
                return;
            }

            string sourcePath = Path.Combine(packageInfo.resolvedPath, sourceFolder);

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
