using UnityEngine;

namespace UnityGameStarter.Gameplay.UI 
{
    public static class UILibrary
    {
        public static bool IsCanvasRoot(this RectTransform rect)
            => rect.TryGetComponent<Canvas>(out _);

        public static bool IsUI(this RectTransform rect)
            => rect.GetComponentInParent<Canvas>() != null;

        public static bool IsCanvasChild(this RectTransform rect)
            => rect.GetComponentInParent<Canvas>() != null && !rect.TryGetComponent<Canvas>(out _);

        public static bool IsTopLevelUI(this RectTransform rect)
            => rect.parent == null || rect.parent.GetComponentInParent<RectTransform>() == null;

        public static bool IsWorldSpaceUI(this RectTransform rect)
        {
            Canvas canvas = rect.GetComponentInParent<Canvas>();

            return (canvas != null && canvas.renderMode == RenderMode.WorldSpace);
        }
    }
}