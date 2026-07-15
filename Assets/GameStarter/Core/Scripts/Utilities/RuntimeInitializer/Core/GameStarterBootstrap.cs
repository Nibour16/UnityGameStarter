using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityGameStarter.RuntimeCore 
{
    static class GameStarterBootstrap
    {
        private readonly struct Initializer
        {
            public readonly RuntimeInitializeLoadType loadType;
            public readonly int order;
            public readonly Action action;

            public Initializer(RuntimeInitializeLoadType loadType, int order, Action action)
            {
                this.loadType = loadType;
                this.order = order;
                this.action = action;
            }
        }

        private static readonly List<Initializer> _initializers = new();
        private static bool _discovered;

        private static int GetLoadOrder(RuntimeInitializeLoadType type)
        {
            return type switch
            {
                RuntimeInitializeLoadType.BeforeSplashScreen => 0,
                RuntimeInitializeLoadType.SubsystemRegistration => 1,
                RuntimeInitializeLoadType.AfterAssembliesLoaded => 2,
                RuntimeInitializeLoadType.BeforeSceneLoad => 3,
                RuntimeInitializeLoadType.AfterSceneLoad => 4,
                _ => int.MaxValue
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void BeforeSplashScreen()
        {
            Execute(RuntimeInitializeLoadType.BeforeSplashScreen);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            Reset();
            Discover();

            Execute(RuntimeInitializeLoadType.SubsystemRegistration);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void AfterAssembliesLoaded()
        {
            Execute(RuntimeInitializeLoadType.AfterAssembliesLoaded);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            Execute(RuntimeInitializeLoadType.BeforeSceneLoad);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            Execute(RuntimeInitializeLoadType.AfterSceneLoad);
        }

        private static void Execute(RuntimeInitializeLoadType loadType)
        {
            foreach (var initializer in _initializers)
            {
                if (initializer.loadType != loadType) continue;
                initializer.action();
            }
        }

        private static void Reset() 
        {
            _initializers.Clear();
            _discovered = false;
        }

        private static void Discover()
        {
            if (_discovered) return;

            _initializers.Clear();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attribute = method.GetCustomAttribute<GameStarterAttribute>();

                        if (attribute == null) continue;

                        if (method.ReturnType != typeof(void)) continue;

                        if (method.GetParameters().Length != 0) continue;

                        var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                        _initializers.Add(new Initializer(attribute.LoadType, attribute.Order, action));
                    }
                }
            }

            _initializers.Sort((a, b) =>
            {
                int phase = GetLoadOrder(a.loadType).CompareTo(GetLoadOrder(b.loadType));   // Ordered by type
                return phase != 0? phase : a.order.CompareTo(b.order);  // Ordered by value if same type
            });

            _discovered = true;
        }
    }
}