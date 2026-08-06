using System;
using System.IO;
using UnityEditor;

namespace UnityGameStarter.EditorUtilities.ScriptCreator 
{
    /// <summary>
    /// Data of the script creator
    /// </summary>
    public class MultipleScriptCreatorData 
    {
        public ScriptCreatorData primaryFile;

        public ScriptCreatorData[] secondaryFiles = Array.Empty<ScriptCreatorData>();
    }

    public class ScriptCreatorData
    {
        public string fileName;
        public object[] templateArgs;
    }

    /// <summary>
    /// Base class of all script creators
    /// 
    /// IMPORTANT:
    /// Unity MenuItem cannot be inherited or auto-registered.
    /// Each derived creator MUST provide its own static MenuItem entry.
    /// </summary>
    public abstract class BaseScriptCreator
    {
        /// <summary>
        /// All children must offer the script module
        /// </summary>
        protected abstract string Template { get; }

        protected virtual string[] SecondaryTemplates => Array.Empty<string>();

        /// <summary>
        /// All children will have the same method for creating entrance - creating script
        /// </summary>
        public void CreateScript(MultipleScriptCreatorData creatorData)
        {
            string folder = GetSelectedFolderPath();

            if (string.IsNullOrEmpty(creatorData.primaryFile.fileName))
                return;

            TryCreateFile(folder, creatorData.primaryFile.fileName, 
                Template, creatorData.primaryFile.templateArgs, out var path);

            if (SecondaryTemplates.Length != creatorData.secondaryFiles.Length)
                throw new Exception("Secondary templates and file names count mismatch.");

            for (int i = 0; i < SecondaryTemplates.Length; i++)
            {
                ScriptCreatorData secondary = creatorData.secondaryFiles[i];

                TryCreateFile(folder, secondary.fileName,
                    SecondaryTemplates[i], secondary.templateArgs, out _);
            }

            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        }

        /// <summary>
        /// Logic to calculate file creation
        /// </summary>
        private bool TryCreateFile(string folder, string fileName, string template, object[] args, out string path)
        {
            path = Path.Combine(folder, fileName + ".cs");

            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                    "Error", $"File '{fileName}.cs' already exists!", "OK");

                return false;
            }

            string content = string.Format(template, args);

            File.WriteAllText(path, content);
            return true;
        }

        /// <summary>
        /// Get current selected folder path (same as Unity creating C# script)
        /// </summary>
        private string GetSelectedFolderPath()
        {
            string folder = "Assets";

            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (Directory.Exists(path))
                    folder = path;
                else
                    folder = Path.GetDirectoryName(path);
            }

            return folder;
        }
    }
}