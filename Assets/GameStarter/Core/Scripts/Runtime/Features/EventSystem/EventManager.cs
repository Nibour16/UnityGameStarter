using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Events
{
    /// <summary>
    /// The Event Listener attribute for methods in the event listener classes
    /// 
    /// Rules for event listener classes inheritance:
    /// - Methods with [EventListener] attribute in a base class can be inherited and registered.
    /// - Private methods with [EventListener] attribute in a base class are not counted as
    ///   inherited listeners in child classes.
    /// </summary>
    /// 
    /// <Remark>
    /// Note that this inheritance support applies to listener methods, not event types.
    /// An EventListener for BaseEvent does NOT automatically receive ChildEvent,
    /// even when ChildEvent inherits from BaseEvent.
    /// 
    /// A valid EventListener method must have exactly one parameter, which is treated as an Event Type
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute { }

    /// <summary>
    /// Manager for registering, unregistering, and publishing events, followed by Event Bus Pattern.
    /// </summary>
    [RuntimeSingleton(-300)]
    public sealed class EventManager : Singleton<EventManager>
    {
        // Event Invoker Data, that stores all events with specific types
        private readonly Dictionary<Type, List<IEventInvoker>> _events = new();

        #region Event Invoker Data Structure
        /// <summary>
        /// Interface that used for data collection 
        /// Specific achievement are handled by the following EventInvoker<T> class
        /// Can offer the Callback delegate, or invoke this event
        /// </summary>
        private interface IEventInvoker
        {
            Delegate Callback { get; }
            void Invoke(object e);
        }

        /// <summary>
        /// Class to handle how an event Invokes
        /// </summary>
        private sealed class EventInvoker<T> : IEventInvoker
        {
            private readonly Action<T> _callback;
            public Delegate Callback => _callback;

            public EventInvoker(Action<T> callback)
            {
                _callback = callback;
            }

            public void Invoke(object e)
            {
                _callback((T)e);
            }
        }
        #endregion

        /// <summary>
        /// Clear all data when this manager is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            ClearAll();
            base.OnDestroy();
        }

        public int GetListenerCount<TEvent>()
        {
            var type = typeof(TEvent);

            if (!_events.TryGetValue(type, out var list))
                return 0;

            return list.Count;
        }

        #region Registration
        public void Register(object listener) => ProcessListener(listener, (type, callback) => 
        { 
            Subscribe(type, callback); 
        });

        public void Unregister(object listener) => ProcessListener(listener, (type, callback) => 
        {
            Unsubscribe(type, callback); 
        });
        #endregion

        /// <summary>
        /// Publish (invoke) all events with a specific event type
        /// If the manager is disabled, the publish won't be executed
        /// </summary>
        public void Publish<TEvent>(TEvent e, bool removePublishedEvent = false)
        {
            if (!enabled) return;
            
            Type eventType = typeof(TEvent);

            if (!_events.TryGetValue(eventType, out var invokers)) return;

            foreach (var listener in invokers)
                listener.Invoke(e);

            if (removePublishedEvent)
                Clear<TEvent>();
        }

        public void Clear<TEvent>() => _events.Remove(typeof(TEvent));
        public void ClearAll() => _events.Clear();

        #region Record event listeners
        private void Subscribe(Type eventType, Delegate listener)
        {
            if (!_events.TryGetValue(eventType, out var list))
            {
                list = new List<IEventInvoker>();
                _events[eventType] = list;
            }

            var invoker = (IEventInvoker)Activator.CreateInstance(
                typeof(EventInvoker<>).MakeGenericType(eventType), listener);

            list.Add(invoker);
        }

        private void Unsubscribe(Type eventType, Delegate listener)
        {
            if (!_events.TryGetValue(eventType, out var list)) return;

            list.RemoveAll(x => x.Callback.Equals(listener));

            if (list.Count == 0)
                _events.Remove(eventType);
        }
        #endregion

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