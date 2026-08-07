using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.StateMachine.EditorUtilities
{
    public class StateMachineCreatorWindow : ScriptCreatorWindow<StateMachineScriptCreator>
    {
        protected override string Title => "State Machine Creator";

        protected override string FileNameLabel => "State Machine Name";

        protected override ContentDefinition File => new() { value = "NewStateMachine" };
    }
}