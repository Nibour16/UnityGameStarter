namespace UnityGameStarter.EditorUtilities.Creator.StateMachine
{
    public class StateMachineCreatorWindow : ScriptCreatorWindow<StateMachineScriptCreator>
    {
        protected override string Title => "State Machine Creator";
        protected override string FilesLabel => "State Machine Name";

        protected override ContentDefinition Files => new() { value = "NewStateMachine" };
    }
}