using UnityEngine;

namespace UnityGameStarter.EventSystem 
{
    public abstract class AutoEvent<T, U> : MonoBehaviour, IEventListener<U> 
        where T : MonoBehaviour where U : AutoEvent<T, U> 
    {
        private T _owner;
        protected T Owner => _owner;
        
        protected virtual void Awake() 
        {
            _owner = GetComponent<T>();

            if (!_owner) Debug.LogError("Owner is not found in the object!");
        }

        protected virtual void OnEnable()
        {
            EventManager.Instance.Register(this);
        }

        protected virtual void OnDisable()
        {
            EventManager.Instance.Unregister(this);
        }

        public abstract void OnEvent(U e);
    }
}