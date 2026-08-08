using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace UnityGameStarter.EditorUtilities.ScriptCreator 
{
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

            List<ScriptCreatorData> files = new() { creatorData.primaryFile };

            files.AddRange(creatorData.secondaryFiles);

            if (!ValidateFiles(folder, files))
                return;

            string path = CreateFile(folder, creatorData.primaryFile.fileName,
                Template, creatorData.primaryFile.templateArgs);

            for (int i = 0; i < SecondaryTemplates.Length; i++)
            {
                CreateFile(folder, creatorData.secondaryFiles[i].fileName,
                    SecondaryTemplates[i], creatorData.secondaryFiles[i].templateArgs);
            }

            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        }

        /// <summary>
        /// Logic to calculate file creation
        /// </summary>
        private string CreateFile(string folder, string fileName, string template, object[] args)
        {
            string path = Path.Combine(folder, fileName + ".cs");

            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                    "Error", $"File '{fileName}.cs' already exists!", "OK");

                return "";
            }

            string content = string.Format(template, args);

            File.WriteAllText(path, content);
            return path;
        }

        /// <summary>
        /// Ensure files are all valid to prevent creating dirty files
        /// </summary>
        private bool ValidateFiles(string folder, IEnumerable<ScriptCreatorData> files)
        {
            foreach (var file in files)
            {
                string path = Path.Combine(folder, file.fileName + ".cs");

                if (File.Exists(path))
                {
                    EditorUtility.DisplayDialog(
                        "Error", $"File '{file.fileName}.cs' already exists!", "OK");

                    return false;
                }
            }

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