using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;
using System.Collections.Generic;

namespace UnityGameStarter.InputSystem.EditorUtilities
{
    public class InputManagerCreatorWindow : ScriptCreatorWindow<InputManagerCreator>
    {
        protected override string Title => "Input Manager Creator";

        protected override string FileNameLabel => "Class Name";

        protected override ContentDefinition File => new() { value = "NewInputManager" };

        protected override Dictionary<string, ContentDefinition> InitialCreatorContent => new()
        {
            { "Inputs Class", new() { value = "DefaultInputs"} }
        };
    }
}