using UnityEngine;

namespace UnityGameStarter.Events.EventManagement
{   
    public interface IAutoEventListener { }

    public sealed class EventListenerRegister : MonoBehaviour
    {
        [SerializeField] private bool printLog = false;
        private IAutoEventListener[] _listeners;

        private bool _isRegistered = false;

        private void Awake() 
        {
            _listeners = GetComponents<IAutoEventListener>();
        }

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        private void Register() 
        {
            if (_isRegistered) return;
            
            if (!EventManager.TryGetInstance(out var instance)) return;
            
            foreach (var listener in _listeners)
            {
                instance.Register(listener);

                if (printLog)
                    Debug.Log($"{listener} has registered");
            }

            _isRegistered = true;
        }

        private void Unregister() 
        {
            if (!_isRegistered) return;
            
            if (!EventManager.TryGetInstance(out var instance)) return;

            foreach (var listener in _listeners)
            {
                instance.Unregister(listener);

                if (printLog)
                    Debug.Log($"{listener} has unregistered");
            }

            _isRegistered = false;
        }
    }
}