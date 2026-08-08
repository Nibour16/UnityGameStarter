using System;

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

    public class MultipleScriptCreatorData
    {
        public ScriptCreatorData primaryFile;

        public ScriptCreatorData[] secondaryFiles = Array.Empty<ScriptCreatorData>();
    }
}