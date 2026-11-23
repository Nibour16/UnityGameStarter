using UnityEngine;
using UnityGameStarter.EditorUtilities.EditorProcessor;

namespace UnityGameStarter.UI.EditorUtilities 
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