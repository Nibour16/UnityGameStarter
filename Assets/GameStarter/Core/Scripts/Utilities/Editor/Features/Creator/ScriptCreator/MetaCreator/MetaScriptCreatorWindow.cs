using System.Collections.Generic;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.EditorUtilities.ScriptCreator 
{
    public class MetaScriptCreatorWindow : ScriptCreatorWindow<MetaScriptCreator>
    {
        protected override string Title => "Meta Script Creator";

        protected override string FileNameLabel => "Creator Name";

        protected override ContentDefinition File => new() 
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