using System.Collections.Generic;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.EditorWindowUtilities.Creator 
{
    public abstract class ScriptCreatorWindow : CreatorWindow
    {
        protected override void OnCreate(Dictionary<string, ContentDefinition> values)
        {
            GetScriptCreator(values).CreateScript();
        }

        protected abstract BaseScriptCreator GetScriptCreator(Dictionary<string, ContentDefinition> values);
    }
}