using System.Collections.Generic;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.EditorUtilities.ScriptCreator 
{
    public sealed class MetaScriptCreatorWindow : ScriptCreatorWindow<MetaScriptCreator>
    {
        protected override bool MakeSecondaryFileValuesEditable => true;
        
        protected override string Title => "Meta Script Creator";

        protected override string FilesLabel => "Creator Files";

        protected override ContentDefinition Files => new() 
        {
            value = "NewScriptCreator",

            secondaryValues = new[]
            {
                "NewScriptCreatorWindow"
            }
        };

        protected override Dictionary<string, ContentDefinition> InitialCreatorContent => new()
        {
            { "Creator Window", new() { value = "NewCreatorWindow" } },
            { "Target Script", new() { value = "DefaultScript"} },
            { "Creator Window Title", new() { value = "New Creator Window"} }
        };
    }
}