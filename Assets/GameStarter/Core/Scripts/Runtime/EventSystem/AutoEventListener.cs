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
            EventManager.Instance.Unregister(this);
        }
    }
}