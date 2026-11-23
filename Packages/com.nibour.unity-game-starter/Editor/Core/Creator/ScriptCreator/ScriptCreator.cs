using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace UnityGameStarter.EditorUtilities.Creator 
{
    /// <remarks>
    /// Unity MenuItem cannot be inherited or auto-registered.
    /// Each derived creator MUST provide its own static MenuItem entry method.
    /// </remarks>
    public abstract class ScriptCreator
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

            if (!ValidateFiles(folder, files)) return;

            string path = CreateFile(folder, creatorData.primaryFile.fileName,
                Template, creatorData.primaryFile.templateArgs);

            for (int i = 0; i < SecondaryTemplates.Length; i++)
            {
                CreateFile(folder, creatorData.secondaryFiles[i].fileName,
                    SecondaryTemplates[i], creatorData.secondaryFiles[i].templateArgs);
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        }

        private string CreateFile(string folder, string fileName, string template, object[] args)
        {
            if (!ValidateFile(folder, fileName, out var path)) return "";

            string content = string.Format(template, args);

            File.WriteAllText(path, content);
            return path;
        }

        /// <summary>
        /// Ensure files are all valid to prevent creating dirty files
        /// </summary>
        private bool ValidateFiles(string folder, IEnumerable<ScriptCreatorData> files)
        {
            HashSet<string> fileNames = new(StringComparer.OrdinalIgnoreCase);

            foreach (var file in files)
            {
                if (!fileNames.Add(file.fileName))
                {
                    EditorUtility.DisplayDialog("Error", $"Duplicate file name '{file.fileName}.cs'!", "OK");
                    return false;
                }

                if (!ValidateFile(folder, file.fileName, out _))
                    return false;
            }

            return true;
        }

        private bool ValidateFile(string folder, string fileName, out string path) 
        {
            path = Path.Combine(folder, fileName + ".cs");

            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog("Error", $"File '{fileName}.cs' already exists!", "OK");
                return false;
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