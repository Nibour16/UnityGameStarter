using System.Collections.Generic;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

public class InputFacadeCreatorWindow : ScriptCreatorWindow<InputFacadeCreator>
{
    protected override string Title => "Input Facade Creator";

    protected override string FilesLabel => "Class Name";

    protected override ContentDefinition Files => new() { value = "NewInputFacade" };

    protected override Dictionary<string, ContentDefinition> InitialCreatorContent => new()
    {
        
    };
}