using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.Events;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Events
{
    /// <summary>
    /// The Event Listener attribute for methods in the event listener classes.
    /// 
    /// Rules for event listener classes inheritance:
    /// - Methods with [EventListener] attribute in a base class can be inherited and registered.
    /// - Private methods with [EventListener] attribute in a base class are not counted as
    ///   inherited listeners in child classes.
    /// </summary>
    /// 
    /// <remarks>
    /// Note that this inheritance support applies to listener methods, not event types.
    /// 
    /// An EventListener for BaseEvent does NOT automatically receive ChildEvent,
    /// even when ChildEvent inherits from BaseEvent
    /// 
    /// A valid EventListener method must have exactly one parameter, which is treated as an Event Type.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute { }

    /// <summary>
    /// TODO: created a completed persistent event registry system to make Event Manager supports it.
    /// 
    /// Manager for registering, unregistering, and publishing events, followed by Event Bus Pattern.
    /// The event manager constructs mainly for cross-system publishment.
    /// Role will be acted as an extension of Unity Events.
    /// </summary>
    [RuntimeSingleton(-300)]
    public sealed class EventManager : Singleton<EventManager>
    {
        /// <summary>
        /// Event Invoker Data, that stores all events with specific types
        /// </summary>
        private readonly Dictionary<Type, IRuntimeEventInvoker> _events = new();
        private readonly Dictionary<Type, IPersistentEventInvoker> _serializedEvents = new();

        /// <summary>
        /// Clear all data when this manager is destroyed
        /// </summary>
        protected override void OnDestroy()
        {
            _events.Clear();
            base.OnDestroy();
        }

        public int GetListenerCount<TEvent>()
        {
            var type = typeof(TEvent);

            return _events.TryGetValue(type, out var invoker)? invoker.Count : 0;
        }

        #region Registration
        /// <summary>
        /// Used the ProcessListener method to register listeners,
        /// and the processor is <see cref="Subscribe"/>
        /// </summary>
        /// <remarks>
        /// To further understand more about the ProcessListener, see <see cref="ProcessListener"/>
        /// </remarks>
        public void Register(object listener) 
        {
            if (!enabled) return;
            
            ProcessListener(listener, Subscribe);
        }

        /// <summary>
        /// Used the ProcessListener method to unregister listeners,
        /// and the processor is <see cref="Unsubscribe"/>
        /// </summary>
        /// <remarks>
        /// To further understand more about the ProcessListener, see <see cref="ProcessListener"/>
        /// </remarks>
        public void Unregister(object listener) 
        {
            if (!enabled) return;

            ProcessListener(listener, Unsubscribe);
        }
        #endregion

        /// <summary>
        /// Publish (invoke) all events with a specific event type.
        /// If the manager is disabled, the publish won't be executed
        /// </summary>
        public void Publish<TEvent>(TEvent e, bool removePublishedEvent = false)
        {
            if (!enabled) return;
            
            Type eventType = typeof(TEvent);

            if (!_events.TryGetValue(eventType, out var invoker)) return;

            invoker.Invoke(e);

            if (removePublishedEvent)
                Clear<TEvent>();
        }

        #region Event Data Clear API
        public void Clear<TEvent>() 
        {
            if (!enabled) return;

            _events.Remove(typeof(TEvent));
        }

        public void ClearAll() 
        {
            if (!enabled) return;

            _events.Clear();
        }
        #endregion

        #region Record event listeners
        private void Subscribe(Type eventType, Delegate listener)
        {
            if (!_events.TryGetValue(eventType, out var invoker))
            {
                // The new invoker will be created as an instance from the Activator
                // The new invoker type must be a generic type of the event type
                invoker = (IRuntimeEventInvoker)Activator.CreateInstance(
                    typeof(EventInvoker<>).MakeGenericType(eventType));

                _events[eventType] = invoker;
            }

            invoker.AddListener(listener);
        }

        private void Unsubscribe(Type eventType, Delegate listener)
        {
            if (!_events.TryGetValue(eventType, out var invoker)) return;

            invoker.RemoveListener(listener);

            if (invoker.Count == 0)
                _events.Remove(eventType);
        }
        #endregion

        /// <summary>
        /// Collect all [EventListener] methods from the <paramref name="listener"/>
        /// to be processed throught the <paramref name="processor"/> action
        /// </summary>
        private void ProcessListener(object listener, Action<Type, Delegate> processor)
        {
            // Get All methods from the listener, excluding static methods
            var methods = listener.GetType().GetMethods
                (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                // All listener methods must be assigned by [EventListener] attribute
                if (method.GetCustomAttribute<EventListenerAttribute>() == null)
                    continue;

                var parameter = method.GetParameters();

                // All listener methods must have exactly one parameter
                if (parameter.Length != 1) 
                {
                    Debug.LogError(
                        "Event Manager: Invalid listener detected: method must contain only one parameter");
                    continue; 
                }

                // This parameter will be acted as the event type
                // The type can be anything, but recommended to make them more understandable
                Type eventType = parameter[0].ParameterType;

                // Define the method info into callback by creating the delegate,
                // The callback type must be the Unity Action with the generic type of the event type
                Delegate callback =
                    method.CreateDelegate(typeof(UnityAction<>).MakeGenericType(eventType), listener);

                // Execute the processor with the event type and the callback
                processor(eventType, callback);
            }
        }
    }
}