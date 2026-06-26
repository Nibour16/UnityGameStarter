using System.Collections.Generic;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities.Creator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.StateMachine 
{
    public class StateMachineCreatorWindow : ScriptCreatorWindow
    {
        protected override string Title => "State Machine Creator";

        protected override string FileNameLabel => "State Machine Name";

        protected override Dictionary<string, ContentDefinition> Content => new()
        {
            { FileNameLabel, new ContentDefinition { value = "NewStateMachine" } }
        };

        protected override BaseScriptCreator GetScriptCreator()
        {
            return new StateMachineScriptCreator();
        }
    }
}