using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.StateMachine.EditorUtilities
{
    public class StateCreatorWindow : ScriptCreatorWindow<StateScriptCreator>
    {
        protected override string Title => "State Creator";

        protected override string FilesLabel => "State Name";

        protected override ContentDefinition Files => new() { value = "NewState" };
    }
}