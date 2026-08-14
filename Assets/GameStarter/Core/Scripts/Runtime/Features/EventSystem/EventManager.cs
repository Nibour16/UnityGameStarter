using System;
using System.Reflection;

using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Events.EventManagement
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute { }

    /// <summary>
    /// Central event manager for registering, unregistering, and publishing events.
    ///
    /// Listener class inheritance is supported:
    /// - [EventListener] methods declared in a base class can be inherited and
    ///   registered when a derived listener is registered.
    /// - Inherited listener methods must be <c>protected</c> or more accessible
    ///   (e.g. <c>protected</c>, <c>public</c>, or <c>internal</c> where applicable).
    /// - Private [EventListener] methods declared in a base class are not considered
    ///   inherited listeners for derived classes.
    ///
    /// Note that this inheritance support applies to listener methods, not event types.
    /// An EventListener for BaseEvent does NOT automatically receive ChildEvent,
    /// even when ChildEvent derives from BaseEvent.
    ///
    /// A valid EventListener method must have exactly one parameter.
    /// The parameter type is treated as the event type and the method is registered
    /// as an Action&lt;TEvent&gt; callback.
    /// </summary>
    [RuntimeSingleton(-300)]
    public sealed class EventManager : Singleton<EventManager>
    {
        private void OnDisable()
        {
            EventBus.ClearAll();
        }

        public int GetListenerCount<TEvent>() => EventBus.GetListenerCount<TEvent>();

        public void Register(object listener)
        {
            ProcessListener(listener, (type, callback) => { EventBus.Subscribe(type, callback); });
        }

        public void Unregister(object listener)
        {
            ProcessListener(listener, (type, callback) => { EventBus.Unsubscribe(type, callback); });
        }

        public void Publish<TEvent>(TEvent e) => EventBus.Publish(e);

        public void PublishAndClear<TEvent>(TEvent e) => EventBus.PublishAndClear(e);

        public void Clear<TEvent>() => EventBus.Clear<TEvent>();

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