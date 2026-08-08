using UnityEngine;

namespace UnityGameStarter.Gameplay.UI 
{
    public abstract class UIBehaviour : MonoBehaviour, IUIComponent
    {
        public virtual void Init() 
        {
            if (gameObject.activeSelf)
                OnOpened();
            else
                OnClosed();
        }

        public virtual void Deinit() 
        {
            OnClosed();
        }

        protected virtual void OnEnable() 
        {
            OnOpened();
        }

        protected virtual void OnDisable()
        {
            OnClosed();
        }

        public abstract void OnOpened();

        public abstract void OnClosed();
    }
}