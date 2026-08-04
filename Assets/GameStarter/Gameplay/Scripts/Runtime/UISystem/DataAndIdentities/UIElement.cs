using UnityEngine;

namespace UnityGameStarter.Gameplay.UI 
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIElement : MonoBehaviour
    {
        public RectTransform Rect => (RectTransform)transform;
    }
}