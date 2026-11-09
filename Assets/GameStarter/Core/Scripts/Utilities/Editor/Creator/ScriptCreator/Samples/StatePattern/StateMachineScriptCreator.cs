using UnityEditor;

namespace UnityGameStarter.EditorUtilities.Creator.StateMachine
{
    public class StateMachineScriptCreator : ScriptCreator
    {
        protected override string Template =>
@"using System;
using UnityEngine;
using UnityGameStarter.FiniteStateMachine;        

public class {0} : BaseStateMachine
{{
    protected override Type[] GetInitialStates()
    {{
        // TODO: Prepate the desired states here, example:
        // return new[] 
        // {{
        //    typeof(StateA),
        //    typeof(StateB)
        // }};

        throw new NotImplementedException();
    }}
}}";

        [MenuItem("Assets/Create/Scripting/State Pattern/State Machine")]
        private static void CreateStateMachine()
        {
            EditorWindowWithInputs.ShowWindow<StateMachineCreatorWindow>();
        }
    }
}