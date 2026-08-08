using UnityEngine;

namespace UnityGameStarter.Events.EventManagement
{   
    public interface IAutoEventListener { }

    public sealed class EventListenerRegister : MonoBehaviour
    {
        [SerializeField] private bool printLog = false;
        private IAutoEventListener[] _listeners;
        
        private void Awake() 
        {
            _listeners = GetComponents<IAutoEventListener>();
        }

        private void OnEnable()
        {
            foreach (var listener in _listeners) 
            {
                EventManager.Instance.Register(listener);

                if (printLog)
                    Debug.Log($"{listener} has registered");
            }
        }

        private void OnDisable()
        {
            if (!EventManager.TryGetInstance(out var instance)) return;

            foreach (var listener in _listeners) 
            {
                instance.Unregister(listener);

                if (printLog)
                    Debug.Log($"{listener} has unregistered");
            }
        }
    }
}