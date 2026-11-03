using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityGameStarter.Events 
{
    /// <summary>
    /// Interface that used for data collection
    /// Used to be handled easily by the data owner that does not require to define the generic type
    /// Specific achievement are handled by the following EventInvoker<T> class
    /// Can offer the Callback delegate, or invoke this event
    /// </summary>
    internal interface IEventInvoker
    {
        int Count { get; }
    }

    internal interface IRuntimeEventInvoker : IEventInvoker 
    {
        void AddListener(Delegate callback);
        void RemoveListener(Delegate callback);
        void Invoke(object e);
    }

    internal interface IPersistentEventInvoker : IEventInvoker { }

    /// <summary>
    /// Class to handle how an event Invokes
    /// Uses Unity Event as the data, to ensure it can be connected by the IEventInvoker interface
    /// </summary>
    internal class EventInvoker<T> : IRuntimeEventInvoker
    {
        private readonly UnityEvent<T> _event = new();
        private readonly List<UnityAction<T>> _listeners = new();

        public int Count => _listeners.Count;

        public void AddListener(Delegate callback)
        {
            var action = (UnityAction<T>)callback;

            _event.AddListener(action);
            _listeners.Add(action);
        }

        public void RemoveListener(Delegate callback)
        {
            var action = (UnityAction<T>)callback;

            _event.RemoveListener(action);

            _listeners.RemoveAll(x => x.Target == action.Target && x.Method == action.Method);
        }

        public void Invoke(object e) => _event.Invoke((T)e);
    }

    public abstract class SerializableEventInvoker<T> : MonoBehaviour, IPersistentEventInvoker
    {
        [SerializeField] private UnityEvent<T> serializedEvent;

        public int Count => serializedEvent.GetPersistentEventCount();

        internal UnityEvent<T> Event => serializedEvent;
    }
}