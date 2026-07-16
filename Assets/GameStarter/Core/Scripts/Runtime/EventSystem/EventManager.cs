using System;
using System.Reflection;

using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.EventSystem.EventManagement
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute { }

    [RuntimeSingleton(-300)]
    public sealed class EventManager : Singleton<EventManager>
    {
        private void OnDisable()
        {
            EventBus.Clear();
        }

        public void Register(object listener)
        {
            ProcessListener(listener, (type, callback) => { EventBus.Subscribe(type, callback); });
        }

        public void Unregister(object listener)
        {
            ProcessListener(listener, (type, callback) => { EventBus.Unsubscribe(type, callback); });
        }

        public void Publish<TEvent>(TEvent e) => EventBus.Publish(e);

        private void ProcessListener(object listener, Action<Type, Delegate> action)
        {
            var methods = listener.GetType().GetMethods
                (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<EventListenerAttribute>() == null)
                    continue;

                var parameter = method.GetParameters();

                if (parameter.Length != 1) 
                {
                    Debug.LogError(
                        "Event Manager: Invalid listener detected: method must contain only one parameter");
                    continue; 
                }

                Type eventType = parameter[0].ParameterType;

                Delegate callback =
                    method.CreateDelegate(typeof(Action<>).MakeGenericType(eventType), listener);

                action(eventType, callback);
            }
        }
    }
}