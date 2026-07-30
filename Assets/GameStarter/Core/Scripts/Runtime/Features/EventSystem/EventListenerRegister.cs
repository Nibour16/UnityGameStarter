using UnityEngine;

namespace UnityGameStarter.EventSystem.EventManagement
{   
    public interface IAutoEventListener { }

    public sealed class EventListenerRegister : MonoBehaviour
    {
        private IAutoEventListener[] _listeners;
        
        private void Awake() 
        {
            _listeners = GetComponents<IAutoEventListener>();
        }

        private void OnEnable()
        {
            foreach (var listener in _listeners)
                EventManager.Instance.Register(listener);
        }

        private void OnDisable()
        {
            if (!EventManager.HasInstance) return;

            foreach (var listener in _listeners)
                EventManager.Instance.Unregister(listener);
        }
    }
}