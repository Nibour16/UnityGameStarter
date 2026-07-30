using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.StateMachine.EditorUtilities
{
    public class StateCreatorWindow : ScriptCreatorWindow
    {
        protected override string Title => "State Creator";

        protected override string FileNameLabel => "State Name";

        protected override ContentDefinition File => new() { value = "NewState" };

        protected override BaseScriptCreator GetScriptCreator()
        {
            return new StateScriptCreator();
        }
    }
}