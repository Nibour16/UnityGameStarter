using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.EditorUtilities;

namespace UnityGameStarter.TwoDExtension.EditorUtilities
{
    public class SpriteBatcherWindows : EditorWindowWithInputs
    {
        protected override string Title => "Sprite Batcher";

        protected override Vector2 MinimumWindowSize => new(500f, 400f);

        protected override Dictionary<string, ContentDefinition> Content()
        {
            return new Dictionary<string, ContentDefinition>();
        }

        protected override Dictionary<string, ButtonDefinition> Buttons
        {
            get
            {
                return new Dictionary<string, ButtonDefinition>();
            }
        }
    }
}
