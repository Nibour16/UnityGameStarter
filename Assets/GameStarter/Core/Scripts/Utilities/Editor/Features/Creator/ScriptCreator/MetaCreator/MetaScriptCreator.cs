using UnityEditor;
using UnityGameStarter.EditorWindowUtilities;

namespace UnityGameStarter.EditorUtilities.ScriptCreator
{
    public sealed class MetaScriptCreator : BaseScriptCreator
    {
        protected override string Template =>
@"using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

public sealed class {0} : BaseScriptCreator
{{
    protected override string Template => 
@""using UnityEngine;

public class New{2} : MonoBehaviour
{{

}}"";

    [MenuItem(""Assets/Create/Scripting/{2}"")]
    private static void Create()
    {{
        EditorWindowWithInputs.ShowWindow<{1}>();
    }}
}}";

        protected override string[] SecondaryTemplates => new[]
        {
@"using System.Collections.Generic;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

public sealed class {1} : ScriptCreatorWindow<{0}>
{{
    protected override string Title => ""{3}"";

    protected override string FileNameLabel => ""Class Name"";

    protected override ContentDefinition File => new() {{ value = ""New{2}"" }};

    protected override Dictionary<string, ContentDefinition> InitialCreatorContent => new()
    {{
        
    }};
}}"
        };

        [MenuItem("Assets/Create/Scripting/ScriptCreator")]
        private static void CreateCreator()
        {
            EditorWindowWithInputs.ShowWindow<MetaScriptCreatorWindow>();
        }
    }
}