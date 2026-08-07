using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

public class InputFacadeCreator : BaseScriptCreator
{
    protected override string Template => 
@"using UnityEngine;

public class NewInputFacade : MonoBehaviour
{

}";

    [MenuItem("Assets/Create/Scripting/InputFacade")]
    private static void Create()
    {
        EditorWindowWithInputs.ShowWindow<InputFacadeCreatorWindow>();
    }
}