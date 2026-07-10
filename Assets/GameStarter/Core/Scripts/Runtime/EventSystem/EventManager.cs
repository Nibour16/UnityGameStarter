using System;
using System.Collections.Generic;
using System.Reflection;
using UnityGameStarter.EventSystem.CoreLibrary;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap;

namespace UnityGameStarter.EventSystem.EventManagement
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute { }

    [RuntimeSingleton]
    public abstract class EventManager<T> : Singleton<T> where T : EventManager<T>
    {
        private readonly Dictionary<Type, List<object>> _listeners = new();
        protected Dictionary<Type, List<object>> Listeners => _listeners;

        protected virtual void OnEnable() { }

        protected virtual void OnDisable()
        {
            ClearAll();
        }

        public virtual void Register(object listener)
        {
            ProcessListener(listener, (type, callback) => { EventBus.Subscribe(type, callback); });
        }

        public virtual void Unregister(object listener)
        {
            ProcessListener(listener, (type, callback) => { EventBus.Unsubscribe(type, callback); });
        }

        public virtual void Publish<TEvent>(TEvent e) => EventBus.Publish(e);

        public virtual void ClearAll()
        {
            _listeners.Clear();
            EventBus.Clear();
        }

        protected void ProcessListener(object listener, Action<Type, Delegate> action)
        {
            var methods = listener.GetType().GetMethods
                (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<EventListenerAttribute>() == null)
                    continue;

                var parameter = method.GetParameters();

                if (parameter.Length != 1) continue;

                Type eventType = parameter[0].ParameterType;

                Delegate callback =
                    method.CreateDelegate(typeof(Action<>).MakeGenericType(eventType), listener);

                action(eventType, callback);
            }
        }
    }

    public class EventManager : EventManager<EventManager> { }
}