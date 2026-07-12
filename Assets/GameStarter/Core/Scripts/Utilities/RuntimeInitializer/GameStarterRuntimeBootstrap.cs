using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityGameStarter.RuntimeCore 
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GameStarterInitializeAttribute : Attribute
    {
        public int Order { get; }

        public GameStarterInitializeAttribute(int order = 0)
        {
            Order = order;
        }
    }

    static class GameStarterBootstrap
    {
        private readonly struct Initializer
        {
            public readonly int Order { get; }
            public readonly Action Action { get; }

            public Initializer(int order, Action action)
            {
                Order = order;
                Action = action;
            }
        }

        private static readonly List<Initializer> _initializers = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_initializers.Count == 0)
                Discover();

            foreach (var initializer in _initializers)
                initializer.Action();
        }

        private static void Discover()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attribute = method.GetCustomAttribute<GameStarterInitializeAttribute>();

                        if (attribute == null) continue;

                        var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                        _initializers.Add(new Initializer(attribute.Order, action));
                    }
                }
            }

            _initializers.Sort((a, b) => a.Order.CompareTo(b.Order));
        }
    }
}