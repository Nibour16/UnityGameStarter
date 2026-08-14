using UnityEngine;
using UnityGameStarter.Gameplay.Interaction;

namespace UnityGameStarter.UI.Button 
{
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public abstract class ButtonBehaviour : MonoBehaviour, IPointerInteractable
    {
        private Button _button;

        protected UIRoot OwnerRoot => GetComponentInParent<UIRoot>();

        protected RectTransform OwnerMenu 
        {
            get
            {
                var canvas = GetComponentInParent<Canvas>();

                if (canvas == null) return null;

                if (OwnerRoot == null || canvas != OwnerRoot.RootCanvas)
                    return canvas.transform as RectTransform;

                Transform current = transform;

                while (current != null && current.parent != canvas.transform)
                {
                    current = current.parent;
                }

                if (current == null)
                    return null;

                return current as RectTransform;
            }
        }

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnPointerClicked);
        }

        public abstract void OnPointerClicked();

        public virtual void OnHovered() { }

        public virtual void OnUnhovered() { }

        public virtual void OnPointerPressed() { }

        public virtual void OnPointerReleased() { }
    }
}

