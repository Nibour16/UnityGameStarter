using System;
using System.Collections.Generic;

namespace UnityGameStarter.EventSystem
{
    public static class EventBus
    {
        private interface IEventInvoker
        {
            Delegate Callback();
            void Invoke(object e);
        }

        private sealed class EventInvoker<T> : IEventInvoker
        {
            private readonly Action<T> _callback;

            public EventInvoker(Action<T> callback)
            {
                _callback = callback;
            }

            public Delegate Callback() => _callback;

            public void Invoke(object e)
            {
                _callback((T)e);
            }
        }

        private static readonly Dictionary<Type, List<IEventInvoker>> _events = new();

        private static readonly Dictionary<Type, List<IEventInvoker>> _cache = new();

        public static void Subscribe(Type eventType, Delegate listener)
        {
            if (!_events.TryGetValue(eventType, out var list))
            {
                list = new List<IEventInvoker>();
                _events[eventType] = list;
            }

            var invoker = (IEventInvoker)Activator.CreateInstance(
                    typeof(EventInvoker<>).MakeGenericType(eventType), listener);

            list.Add(invoker);

            _cache.Clear();
        }

        public static void Subscribe<T>(Action<T> listener)
        {
            Subscribe(typeof(T), listener);
        }

        public static void Unsubscribe(Type eventType, Delegate listener)
        {
            if (!_events.TryGetValue(eventType, out var list))
                return;


            list.RemoveAll(x => x.Callback().Equals(listener));


            if (list.Count == 0)
                _events.Remove(eventType);


            _cache.Clear();
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            Unsubscribe(typeof(T), listener);
        }

        public static void Publish<TEvent>(TEvent e)
        {
            Type eventType = typeof(TEvent);

            if (!_cache.TryGetValue(eventType, out var listeners))
            {
                listeners = new List<IEventInvoker>();

                foreach (var pair in _events)
                {
                    if (pair.Key.IsAssignableFrom(eventType))
                    {
                        listeners.AddRange(pair.Value);
                    }
                }

                _cache[eventType] = listeners;
            }

            foreach (var listener in listeners)
            {
                listener.Invoke(e);
            }
        }

        public static void PublishAndClear<TEvent>(TEvent e) 
        {
            Publish(e);
            Clear<TEvent>();
        }

        public static int GetListenerCount<TEvent>()
        {
            var type = typeof(TEvent);

            if (!_events.TryGetValue(type, out var list))
                return 0;

            return list.Count;
        }

        public static void Clear<TEvent>()
        {
            _events.Remove(typeof(TEvent));
            _cache.Clear();
        }

        public static void ClearAll()
        {
            _events.Clear();
            _cache.Clear();
        }
    }
}