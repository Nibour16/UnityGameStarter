using System.Collections.Generic;
using System.Linq;

using UnityGameStarter.EditorUtilities.ScriptCreator;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.EditorWindowUtilities.Creator 
{
    using UnityGameStarter.StringLibrary;

    public abstract class ScriptCreatorWindow<T> : CreatorWindow where T : BaseScriptCreator, new()
    {
        protected static readonly T Creator = new();

        protected abstract string FilesLabel { get; }
        protected abstract ContentDefinition Files { get; }
        protected virtual Dictionary<string, ContentDefinition> InitialCreatorContent => new();

        protected override Dictionary<string, ContentDefinition> Content() 
        {
            Dictionary<string, ContentDefinition> result = new()
            {
                { FilesLabel, Files }
            };

            foreach (var pair in InitialCreatorContent)
                result.Add(pair.Key, pair.Value);

            return result;
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
                    fileName = content[FilesLabel].value,
                    templateArgs = args.ToArray()
                },

                secondaryFiles = ParseSecondary(content)
            };

            Creator.CreateScript(creatorData);
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
    }
}