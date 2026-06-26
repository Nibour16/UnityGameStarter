using UnityEngine;
using UnityEditor;
using System.IO;

namespace UnityGameStarter.EditorUtilities.ScriptCreator 
{
    /// <summary>
    /// Data of the script creator
    /// </summary>
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

        /// <summary>
        /// All children will have the same method - creating script
        /// </summary>
        public void CreateScript(ScriptCreatorData creatorData)
        {
            string folder = GetSelectedFolderPath();

            if (string.IsNullOrEmpty(creatorData.fileName))
                return;

            string path = Path.Combine(folder, creatorData.fileName + ".cs");

            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                    "Error", $"File '{creatorData.fileName}.cs' already exists!", "OK");
                return;
            }

            string content = string.Format(Template, creatorData.templateArgs);
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