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
    private PlayerInputs_Core _coreInputs;
    public PlayerInputs_Core CoreInputs => _coreInputs;    

    private {1} _playerInputs;
    public {1} PlayerInputs => _playerInputs;

    private void Awake()
    {{
        _coreInputs = new PlayerInputs_Core();
        _playerInputs = new {1}();
    }}

    private void OnEnable()
    {{
        _coreInputs?.Enable();
        _playerInputs?.Enable();
    }}

    private void OnDisable()
    {{
        _coreInputs?.Disable();
        _playerInputs?.Disable();
    }}
}}";

        [MenuItem("Assets/Create/Scripting/InputManager")]
        private static void CreateInputManager()
        {
            EditorWindowWithInputs.ShowWindow<InputManagerCreatorWindow>();
        }
    }
}