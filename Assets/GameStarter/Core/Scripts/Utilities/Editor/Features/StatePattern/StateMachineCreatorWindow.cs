using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.StateMachine.EditorUtilities
{
    public class StateMachineCreatorWindow : ScriptCreatorWindow<StateMachineScriptCreator>
    {
        protected override string Title => "State Machine Creator";

        protected override string FilesLabel => "State Machine Name";

        protected override ContentDefinition Files => new() { value = "NewStateMachine" };
    }
}