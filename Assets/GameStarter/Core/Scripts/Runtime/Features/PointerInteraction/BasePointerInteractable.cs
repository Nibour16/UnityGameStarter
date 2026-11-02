using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityGameStarter.PointerInteraction 
{
    public abstract class BasePointerInteractable : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public abstract void OnPointerClick(PointerEventData eventData);

        public virtual void OnPointerUp(PointerEventData eventData) { }

        public virtual void OnPointerDown(PointerEventData eventData) { }

        public virtual void OnPointerEnter(PointerEventData eventData) { }

        public virtual void OnPointerExit(PointerEventData eventData) { }
    }
}