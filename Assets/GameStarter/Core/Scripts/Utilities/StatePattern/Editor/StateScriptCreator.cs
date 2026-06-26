using UnityEditor;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities;

namespace UnityGameStarter.StateMachine.EditorUtilities 
{
    public class StateScriptCreator : BaseScriptCreator
    {
        protected override string Template =>
@"using UnityEngine;
using UnityGameStarter.FiniteStateMachine;

public class {0} : BaseState
{{
    public {0}(BaseStateMachine stateMachine) : base(stateMachine) {{ }}

    public override void EnterState()
    {{
        // TODO: Enter logic for {0}
    }}

    public override void UpdateState()
    {{
        // TODO: Update logic for {0}
    }}

    public override void ExitState()
    {{
        // TODO: Exit logic for {0}
    }}
}}";

        [MenuItem("Assets/Create/Scripting/State Pattern/State")]
        private static void CreateState()
        {
            EditorWindowWithInputs.ShowWindow<StateCreatorWindow>();
        }
    }
}