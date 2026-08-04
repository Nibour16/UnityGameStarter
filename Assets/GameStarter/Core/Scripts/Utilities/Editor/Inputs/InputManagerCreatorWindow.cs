using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using System.Collections.Generic;

namespace UnityGameStarter.InputSystem.EditorUtilities
{
    public class InputManagerCreatorWindow : ScriptCreatorWindow
    {
        protected override string Title => "Input Manager Creator";

        protected override string FileNameLabel => "Class Name";

        protected override ContentDefinition File => new() { value = "NewInputManager" };

        protected override BaseScriptCreator GetScriptCreator()
        {
            return new InputManagerCreator();
        }

        protected override Dictionary<string, ContentDefinition> InitialCreatorContent => new()
        {
            { "Inputs Class", new() { value = "DefaultInputs"} }
        };
    }
}