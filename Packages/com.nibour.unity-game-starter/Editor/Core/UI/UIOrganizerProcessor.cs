using UnityEngine;
using UnityGameStarter.EditorUtilities.EditorProcessor;
using UnityGameStarter.UI;

namespace UnityGameStarter.EditorUtilities.UI
{
    public static class UIOrganizerProcessor
    {
        [EditorProcess(-95)]
        private static void Organize() 
        {
            foreach (var organizer in Object.FindObjectsByType<UIOrganizer>())
            {
                if (organizer.AutoOrganize)
                    organizer.Organize();
            }
        }
    }
}