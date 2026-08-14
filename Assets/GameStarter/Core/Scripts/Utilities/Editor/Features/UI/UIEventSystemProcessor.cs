using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

using UnityGameStarter.EditorUtilities.EditorProcessor;

namespace UnityGameStarter.UI.EditorUtilities 
{
    public static class UIEventSystemProcessor
    {
        [EditorProcess(-100)]
        private static void EnsureEventSystem()
        {
            if (Object.FindAnyObjectByType<UIRoot>(FindObjectsInactive.Include) == null)
                return;

            if (Object.FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
                return;

            var go = new GameObject("EventSystem");
            Undo.RegisterCreatedObjectUndo(go, "Create EventSystem");

            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }
    }
}