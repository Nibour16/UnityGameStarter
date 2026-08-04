using UnityEngine;
using UnityEngine.UI;

namespace UnityGameStarter.Gameplay.UI 
{
    public class UIOrganizer : MonoBehaviour
    {
        [SerializeField] private UIRoot root;
        [SerializeField] private bool stripCanvasComponents = true;

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
            Canvas canvas = root.RootCanvas;

            if (canvas == null) return;

            foreach (var rect in rects)
            {
                if (!IsOrganizable(canvas, rect))
                {
                    if (rect != canvas.transform && stripCanvasComponents && rect.IsChildOf(canvas.transform))
                        RemoveCanvasComponents(rect);

                    continue;
                }

                rect.SetParent(canvas.transform, false);

                if (stripCanvasComponents)
                    RemoveCanvasComponents(rect);
            }
        }

        private bool IsOrganizable(Canvas canvas, RectTransform rect)
        {
            var IsNotChildOfDesiredCanvas = false;
            
            if (!rect.IsChildOf(canvas.transform)) 
                IsNotChildOfDesiredCanvas = true;

            if (rect.GetComponentInParent<UIRoot>() != null) return false;

            if (rect.IsWorldSpaceUI()) return false;

            return rect.IsTopLevelUI() || IsNotChildOfDesiredCanvas;
        }

        private void RemoveCanvasComponents(RectTransform rect)
        {
            if (rect.TryGetComponent<CanvasScaler>(out var canvasScaler))
                DestroyImmediate(canvasScaler);
            if (rect.TryGetComponent<GraphicRaycaster>(out var graphicRaycaster))
                DestroyImmediate(graphicRaycaster);
            if (rect.TryGetComponent<Canvas>(out var canvas))
                DestroyImmediate(canvas);
        }
    }
}