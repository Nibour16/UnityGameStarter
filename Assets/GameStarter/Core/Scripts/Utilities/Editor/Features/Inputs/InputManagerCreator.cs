using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

namespace UnityGameStarter.EditorUtilities.InputSystem
{
    public class InputManagerCreator : BaseScriptCreator
    {
        protected override string Template =>
@"using UnityEngine;

public class {0} : MonoBehaviour
{{
    private {1} _inputs;
    
    // TODO: Pass your inputs from your inputs classes as public properties here
    // Example: public bool Move => _inputs.Player.Move.ReadValue<Vector2>();

    private void Awake()
    {{
        _inputs = new {1}();
    }}

    private void OnEnable()
    {{
        _inputs?.Enable();
    }}

    private void OnDisable()
    {{
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