using System.Collections.Generic;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

public class StateMachineCreatorWindow : ScriptCreatorWindow
{
    protected override string Title => "State Machine Creator";

    protected override Dictionary<string, ContentDefinition> Content => new()
    {
        { "State Machine Name", new ContentDefinition { value = "NewStateMachine" } }
    };

    protected override BaseScriptCreator GetScriptCreator(Dictionary<string, ContentDefinition> values)
    {
        return new StateMachineScriptCreator { stateMachineName = values["State Machine Name"].value };
    }
}
