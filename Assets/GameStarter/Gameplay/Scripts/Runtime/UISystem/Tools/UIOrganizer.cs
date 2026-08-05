using UnityEngine;
using UnityEngine.UI;

namespace UnityGameStarter.Gameplay.UI 
{
    public class UIOrganizer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIRoot root;

        [Header("Settings")]
        [SerializeField] private bool stripCanvasComponents = true;
        [SerializeField] private bool normalizeLayout = true;

        /// <Summary>
        /// Automatically organizes UI elements under the UIRoot (e.g. huds, menus).
        /// 
        /// Warning:
        /// May produce unexpected results when multiple UIRoots exist in the same scene.
        /// Disable this option if manual organization is required.
        /// </summary>
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
                if (CanOrganize(rect))
                {
                    rect.SetParent(canvas.transform, false);
                }
                if (!IsResolvable(canvas, rect))
                {
                    continue;
                }

                if (normalizeLayout)
                    NormalizeLayout(rect);

                if (stripCanvasComponents)
                    RemoveCanvasComponents(rect);
            }
        }

        private bool CanOrganize(RectTransform rect)
        {
            if (rect.GetComponentInParent<UIRoot>() != null)
                return false;

            return rect.IsTopLevelUI();
        }

        private bool IsResolvable(Canvas canvas, RectTransform rect) 
        {
            if (rect == canvas.transform) return false;

            if (!rect.IsChildOf(canvas.transform)) return false;

            if (rect.IsWorldSpaceUI()) return false;

            return true;
        }

        private void RemoveCanvasComponents(RectTransform rect)
        {
            if (rect.TryGetComponent<Canvas>(out var canvas)) 
                if (canvas == root.RootCanvas) return;

            if (rect.TryGetComponent<CanvasScaler>(out var canvasScaler))
                DestroyImmediate(canvasScaler);
            if (rect.TryGetComponent<GraphicRaycaster>(out var graphicRaycaster))
                DestroyImmediate(graphicRaycaster);

            if (canvas != null)
                DestroyImmediate(canvas);
        }

        private void NormalizeLayout(RectTransform rect) 
        {
            Transform canvas = root.RootCanvas.transform;

            if (rect == canvas || rect.parent != canvas)
                return;

            rect.Normalize();
        }
    }
}