using System.Collections.Generic;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.InputSystem.EditorUtilities 
{
    public sealed class InputFacadeCreatorWindow : ScriptCreatorWindow<InputFacadeCreator>
    {
        protected override string Title => "Input Facade Creator";

        protected override string FilesLabel => "Class Name";

        protected override ContentDefinition Files => new() { value = "NewInputFacade" };

        protected override Dictionary<string, ContentDefinition> InitialCreatorContent => new()
        {
            { "Manager", new() { value = "InputManager"} }
        };
    }
}