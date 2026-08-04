using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.Gameplay.UI.EditorUtilities 
{
    [CustomEditor(typeof(UIOrganizer))]
    public class UIOrganzierEditor : Editor
    {
        private const string buttonName = "Organize UI";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var organizer = (UIOrganizer)target;

            if (GUILayout.Button(buttonName))
            {
                organizer.Organize();
            }
        }
    }
}