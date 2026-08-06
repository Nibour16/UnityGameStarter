using System.Collections.Generic;
using System.Linq;

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
            var list = new List<KeyValuePair<string, ContentDefinition>> { new(FileNameLabel, File) };

            list.AddRange(InitialCreatorContent);

            return list.ToDictionary(x => x.Key, x => x.Value);
        }

        protected override void OnCreate(Dictionary<string, ContentDefinition> content)
        {
            List<object> args = new();

            foreach (var definition in content.Values)
            {
                args.Add(
                    StringLibrary.Parse(definition.value)
                );
            }

            var creatorData = new MultipleScriptCreatorData
            {
                primaryFile = new ScriptCreatorData
                {
                    fileName = content[FileNameLabel].value,
                    templateArgs = args.ToArray()
                },

                secondaryFiles = ParseSecondary(content)
            };

            GetScriptCreator().CreateScript(creatorData);
        }

        private ScriptCreatorData[] ParseSecondary(Dictionary<string, ContentDefinition> content)
        {
            List<ScriptCreatorData> result = new();

            object[] sharedArgs = content.Values.Select(x => StringLibrary.Parse(x.value)).ToArray();

            foreach (var definition in content.Values)
            {
                if (definition.secondaryValues == null)
                    continue;

                foreach (var value in definition.secondaryValues)
                {
                    result.Add(new ScriptCreatorData
                    {
                        fileName = value,
                        templateArgs = sharedArgs
                    });
                }
            }

            return result.ToArray();
        }

        protected abstract BaseScriptCreator GetScriptCreator();
    }
}