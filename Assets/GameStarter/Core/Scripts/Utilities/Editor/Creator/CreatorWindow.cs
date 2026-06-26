using System.Collections.Generic;

namespace UnityGameStarter.EditorWindowUtilities.Creator 
{
    using UnityGameStarter.EditorWindowUtilities.Data;

    public abstract class CreatorWindow : EditorWindowWithInputs
    {
        protected override Dictionary<string, ButtonDefinition> Buttons => new()
        {
            {
                "Create",
                new ButtonDefinition()
                {
                    closeOnClick = true, onClicked = content => OnCreate(content)
                }
            }
        };

        protected abstract void OnCreate(Dictionary<string, ContentDefinition> content);
    }
}