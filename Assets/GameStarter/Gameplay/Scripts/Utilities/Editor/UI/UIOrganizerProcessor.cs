using UnityEngine;
using UnityEditor;

namespace UnityGameStarter.Gameplay.UI.EditorUtilities 
{
    [InitializeOnLoad]
    public static class UIOrganizerProcessor
    {
        private static bool _organizing = false;
        private static bool _scheduled = false;

        static UIOrganizerProcessor()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            if (_scheduled) return;

            _scheduled = true;

            EditorApplication.delayCall += () =>
            {
                _scheduled = false;
                Organize();
            };
        }

        private static void Organize() 
        {
            if (_organizing) return;

            _organizing = true;

            try
            {
                foreach (var organizer in Object.FindObjectsByType<UIOrganizer>())
                {
                    if (organizer.AutoOrganize)
                        organizer.Organize();
                }
            }
            finally
            {
                _organizing = false;
            }
        }
    }
}