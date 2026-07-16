using UnityEngine;

namespace UnityGameStarter.EventSystem.EventManagement.AutoEventFeature
{
    public abstract class AutoEventListener<TEvent> : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            EventManager.Instance.Register(this);
        }

        protected virtual void OnDisable()
        {
            var manager = EventManager.Instance;

            if (manager)
                manager.Unregister(this);
        }
    }
}