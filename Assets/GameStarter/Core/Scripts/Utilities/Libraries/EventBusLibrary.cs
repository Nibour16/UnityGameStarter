using System;
using System.Collections.Generic;

namespace UnityGameStarter.EventSystem
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        public static void Subscribe<T>(Action<T> listener) // To subscribe an event from specific type
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var del)) // If this type is already suscribed into the data
                _events[type] = Delegate.Combine(del, listener); // Then suscribe the listener to this type
            else
                _events.Add(type, listener); // Otherwise add the type with the listener into it
        }

        public static void Unsubscribe<T>(Action<T> listener) // To unsubscribe an event from specific type
        {
            var type = typeof(T);

            if (!_events.TryGetValue(type, out var del)) return; // Prevent the data does not have this type

            del = Delegate.Remove(del, listener); // Remove the listener from the delegate

            if (del == null) 
                _events.Remove(type); // Remove the type if its delegate has no event collected
            else 
                _events[type] = del; // Update the data otherwise
        }

        public static void Publish<T>(T e) // Publishes an event to all listeners subscribed to this event type.
        {
            if (_events.TryGetValue(typeof(T), out var del))
                ((Action<T>)del)?.Invoke(e);
        }

        public static void Clear()
        {
            _events.Clear();
        }
    }
}