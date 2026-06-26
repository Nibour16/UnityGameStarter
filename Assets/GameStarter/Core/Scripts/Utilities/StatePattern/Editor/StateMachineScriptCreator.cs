using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

namespace UnityGameStarter.StateMachine.EditorUtilities
{
    public class StateMachineScriptCreator : BaseScriptCreator
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