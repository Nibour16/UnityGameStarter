using UnityEngine;

namespace UnityGameStarter.Gameplay.UI 
{
    public abstract class UIBehaviour : MonoBehaviour, IUIComponent
    {
        public virtual void Init() 
        {
            if (gameObject.activeSelf)
                OnOpened();
        }

        public virtual void Deinit() { }

        public abstract void OnOpened();

        public abstract void OnClosed();
    }
}