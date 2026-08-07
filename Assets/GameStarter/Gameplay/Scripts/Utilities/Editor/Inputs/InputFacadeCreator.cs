using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

public sealed class InputFacadeCreator : BaseScriptCreator
{
    protected override string Template =>
@"using UnityEngine;
using UnityGameStarter.ServiceLocatorPattern.FacadeModule;

public sealed class {0} : BaseSingletonFacade
    <{0}, IInputService, {1}>, IInputService
{{
    protected override void Awake()
    {{
        base.Awake();
        EnableDontDestroyOnLoad();
    }}
}}";

    [MenuItem("Assets/Create/Scripting/InputFacade")]
    private static void Create()
    {
        EditorWindowWithInputs.ShowWindow<InputFacadeCreatorWindow>();
    }
}