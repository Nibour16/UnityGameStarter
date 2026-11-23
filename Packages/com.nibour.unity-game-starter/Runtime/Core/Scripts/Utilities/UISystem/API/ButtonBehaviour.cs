using UnityEngine;

using UnityEngine.EventSystems;
using UnityGameStarter.PointerInteraction;

namespace UnityGameStarter.UI.Button 
{
    using UnityEngine.UI;

    /// <summary>
    /// Base behaviour for button interaction with UI navigation support
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBehaviour : BasePointerInteractable,
        ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
    {
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

        #region Pointer Interaction
        public override void OnPointerClick(PointerEventData eventData)
        {
            OnButtonClick(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnButtonHovered(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnButtonUnhovered(eventData);
        }
        #endregion

        public void OnSelect(BaseEventData eventData)
        {
            OnButtonHovered(eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            OnButtonUnhovered(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            OnButtonClick(eventData);
        }

        public abstract void OnButtonClick<T>(T eventData) where T : BaseEventData;

        public virtual void OnMove(AxisEventData eventData) { }
        public virtual void OnCancel(BaseEventData eventData) { }

        public virtual void OnButtonHovered<T>(T eventData) where T : BaseEventData { }
        public virtual void OnButtonUnhovered<T>(T eventData) where T : BaseEventData { }
    }
}

