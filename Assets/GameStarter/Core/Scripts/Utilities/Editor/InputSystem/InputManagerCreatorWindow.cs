namespace UnityGameStarter.EditorUtilities.ScriptCreator.InputSystem 
{
    using UnityGameStarter.EditorUtilities.ScriptCreator.InputSystem;
    using UnityGameStarter.EditorWindowUtilities.Creator;
    using UnityGameStarter.EditorWindowUtilities.Data;

    public class InputManagerCreatorWindow : ScriptCreatorWindow
    {
        protected override string Title => "State Creator";

        protected override string FileNameLabel => "State Name";

        protected override ContentDefinition File => new() { value = "NewState" };

        protected override BaseScriptCreator GetScriptCreator()
        {
            return new InputManagerCreator();
        }
    }
}