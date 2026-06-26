using System.Collections.Generic;
using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.EditorWindowUtilities.Creator 
{
    using UnityGameStarter.StringLibrary;

    public abstract class ScriptCreatorWindow : CreatorWindow
    {
        protected abstract string FileNameLabel { get; }
        protected abstract ContentDefinition File { get; }
        protected virtual Dictionary<string, ContentDefinition> InitialCreatorContent => new();

        protected override Dictionary<string, ContentDefinition> Content() 
        {
            var initialCreatorContent = InitialCreatorContent;
            initialCreatorContent.Add(FileNameLabel, File);
            return initialCreatorContent;
        }

        protected override void OnCreate(Dictionary<string, ContentDefinition> content)
        {
            List<object> args = new();

            foreach (var value in content.Values)
            {
                args.Add(StringLibrary.Parse(value.value));
            }

            var creatorData = new ScriptCreatorData
            {
                fileName = content[FileNameLabel].value,
                templateArgs = args.ToArray()
            };

            GetScriptCreator().CreateScript(creatorData);
        }

        protected abstract BaseScriptCreator GetScriptCreator();
    }
}