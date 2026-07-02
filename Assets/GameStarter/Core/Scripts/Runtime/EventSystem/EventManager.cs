using System;
using System.Collections.Generic;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.EventSystem 
{
    public interface IEventListener<T> 
    {
        void OnEvent(T e);
    }
    
    public abstract class EventManager<T> : Singleton<T> where T : EventManager<T>
    {
        private readonly Dictionary<Type, List<object>> _listeners = new();
        protected Dictionary<Type, List<object>> Listeners => _listeners;

        protected virtual void OnEnable() { }

        protected virtual void OnDisable()
        {
            ClearAll();
        }

        public void Register<U>(IEventListener<U> listener)
        {
            var type = typeof(U);

            if (!_listeners.TryGetValue(type, out var list))
            {
                list = new List<object>();
                _listeners[type] = list;
            }

            list.Add(listener);

            EventBus.Subscribe<U>(listener.OnEvent);
        }

        public void Unregister<U>(IEventListener<U> listener)
        {
            var type = typeof(U);

            if (_listeners.TryGetValue(type, out var list))
                list.Remove(listener);

            EventBus.Unsubscribe<U>(listener.OnEvent);
        }

        public virtual void ClearAll()
        {
            _listeners.Clear();
            EventBus.Clear();
        }
    }

    public class EventManager : EventManager<EventManager> { }
}