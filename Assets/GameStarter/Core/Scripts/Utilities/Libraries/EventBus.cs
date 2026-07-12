using System;
using System.Collections.Generic;

namespace UnityGameStarter.EventSystem
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        /// <summary>
        /// To subscribe an event from specific type
        /// </summary>
        public static void Subscribe(Type eventType, Delegate listener)
        {
            if (_events.TryGetValue(eventType, out var del)) // If this type is already suscribed into the data
                _events[eventType] = Delegate.Combine(del, listener); // Then suscribe the listener to this type
            else
                _events.Add(eventType, listener); // Otherwise add the type with the listener into it
        }

        public static void Subscribe<T>(Action<T> listener) 
        {
            Subscribe(typeof(T), listener);
        }

        /// <summary>
        /// To unsubscribe an event from specific type
        /// </summary>
        public static void Unsubscribe(Type eventType, Delegate listener) 
        {
            if (!_events.TryGetValue(eventType, out var del)) return; // Prevent the data does not have this type

            del = Delegate.Remove(del, listener); // Remove the listener from the delegate

            if (del == null)
                _events.Remove(eventType); // Remove the type if its delegate has no event collected
            else
                _events[eventType] = del; // Update the data otherwise
        }

        public static void Unsubscribe<T>(Action<T> listener) 
        {
            Unsubscribe(typeof(T), listener);
        }

        /// <summary>
        /// To publish an event to all listeners subscribed to this event type
        /// </summary>
        public static void Publish<TEvent>(TEvent e)
        {
            if (_events.TryGetValue(typeof(TEvent), out var del))
                ((Action<TEvent>)del)?.Invoke(e);
        }

        public static void Clear()
        {
            _events.Clear();
        }
    }
}