using UnityEngine;

namespace UnityGameStarter.UI 
{
    /// <summary>
    /// Base UI Behaviour that contains base method to callback from UI life cycle. 
    /// Derived by IUIComponent interface, to define very basic starting behaviour, mainly for better user experiences
    /// </summary>
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