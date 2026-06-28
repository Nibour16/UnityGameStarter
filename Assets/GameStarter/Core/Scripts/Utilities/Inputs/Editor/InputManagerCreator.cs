using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

namespace UnityGameStarter.InputSystem.EditorUtilities 
{
    public class InputManagerCreator : BaseScriptCreator
    {
        protected override string Template =>
@"using UnityGameStarter.InputSystem;

public class {0} : InputManager<{0}>
{{
    private {1} _inputs;
    public {1} Inputs => _inputs;

    protected override void Awake()
    {{
        base.Awake();
        _inputs = new {1}();
    }}

    protected override void OnEnable()
    {{
        base.OnEnable();
        _inputs?.Enable();
    }}

    protected override void OnDisable()
    {{
        base.OnDisable();
        _inputs?.Disable();
    }}
}}";

        [MenuItem("Assets/Create/Scripting/InputManager")]
        private static void CreateInputManager()
        {
            EditorWindowWithInputs.ShowWindow<InputManagerCreatorWindow>();
        }
    }
}