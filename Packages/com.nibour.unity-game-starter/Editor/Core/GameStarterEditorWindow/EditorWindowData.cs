using System;
using System.Collections.Generic;

namespace UnityGameStarter.EditorUtilities
{
    public class ContentDefinition
    {
        public string value;

        public string[] secondaryValues;
    }

    public class ButtonDefinition
    {
        public Action<Dictionary<string, ContentDefinition>> onClicked;
        public bool closeOnClick = false;
        public Func<Dictionary<string, ContentDefinition>, bool> enabledCondition;
    }
}