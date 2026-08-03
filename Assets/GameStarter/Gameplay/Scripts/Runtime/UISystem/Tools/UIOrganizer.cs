using UnityEngine;

namespace UnityGameStarter.Gameplay.UI 
{
    public class UIOrganizer : MonoBehaviour
    {
        [SerializeField] private UIRoot root;

        [Header("Editor")]
        [SerializeField] private bool autoOrganize = false;
        public bool AutoOrganize => autoOrganize;

        public void Organize()
        {
            if (root == null)
            {
                Debug.LogError("UI Organizer: Root is not assigned.");
                return;
            }

            var rects = FindObjectsByType<RectTransform>(FindObjectsInactive.Include);
            Canvas canvas = root.GetComponentInChildren<Canvas>(true);

            foreach (var rect in rects)
            {
                if (!IsOrganizable(rect))
                    continue;

                rect.SetParent(canvas.transform, false);
            }
        }

        private bool IsOrganizable(RectTransform rect)
        {
            if (rect.GetComponentInParent<UIRoot>() != null)
                return false;

            if (rect.IsCanvasRoot()) return false;

            if (rect.IsWorldSpaceUI()) return false;

            return rect.IsTopLevelUI();
        }
    }
}