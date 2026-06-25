using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

public class StateMachineScriptCreator : BaseScriptCreator
{
    public string stateMachineName;

    protected override string Template =>
@"using UnityEngine;

public class {0} : BaseStateMachine
{{
    protected override Type[] GetInitialState()
    {{
        // TODO: Prepate the desired states here, example:
        // return new[] 
        // {
        //    typeof(StateA),
        //    typeof(StateB)
        // };
    }}
}}";

    protected override string GetFileName()
    {
        return stateMachineName;
    }

    protected override object[] GetTemplateArgs()
    {
        return new object[]
        {
            GetFileName()
        };
    }

    [MenuItem("Assets/Create/Scripting/State Machine")]
    private static void CreateStateMachine()
    {
        var creator = new StateMachineScriptCreator();

        EditorWindowWithInputs.ShowWindow<StateMachineCreatorWindow>();
    }
}
