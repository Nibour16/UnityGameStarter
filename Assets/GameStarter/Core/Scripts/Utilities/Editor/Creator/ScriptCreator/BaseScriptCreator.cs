using UnityEngine;
using UnityEditor;
using System.IO;

namespace UnityGameStarter.EditorUtilities.ScriptCreator 
{
    /// <summary>
    /// Base class of all script creators
    /// Only works for defining file path, file creating, and project refresh
    /// </summary>
    public abstract class BaseScriptCreator
    {
        /// <summary>
        /// All children must offer the script module
        /// </summary>
        protected abstract string Template { get; }

        /// <summary>
        /// All children must offer the file name creation method£¨Do not include .cs£©
        /// </summary>
        protected abstract string GetFileName();

        /// <summary>
        /// All children must offer the module parameters
        /// </summary>
        protected abstract object[] GetTemplateArgs();

        /// <summary>
        /// All children will have the same method - creating script
        /// </summary>
        public void CreateScript()
        {
            string folder = GetSelectedFolderPath();
            string fileName = GetFileName();

            if (string.IsNullOrEmpty(fileName))
                return;

            string path = Path.Combine(folder, fileName + ".cs");

            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                    "Error", $"File '{fileName}.cs' already exists!", "OK");
                return;
            }

            string content = string.Format(Template, GetTemplateArgs());
            File.WriteAllText(path, content);

            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<Object>(path);
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