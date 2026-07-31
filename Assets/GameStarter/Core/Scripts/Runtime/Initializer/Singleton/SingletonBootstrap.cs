using System;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityGameStarter.RuntimeCore;

namespace UnityGameStarter.SingletonPattern
{
    public static class SingletonBootstrap
    {
        [GameStarterRuntime(RuntimeInitializeLoadType.SubsystemRegistration, -290)]
        public static void Reset()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && t.GetCustomAttribute<RuntimeSingletonAttribute>() != null);

            foreach (var type in types)
            {
                var method = FindResetMethod(type);

                method?.Invoke(null, null);
            }
        }

        [GameStarterRuntime(RuntimeInitializeLoadType.BeforeSceneLoad, -290)]
        public static void Initialize()
        {
            var types = AppDomain.CurrentDomain
                .GetAssemblies().SelectMany(
                    a => a.GetTypes()).Where(t =>!t.IsAbstract && IsValidRuntimeSingleton(t) &&
                    t.GetCustomAttribute<RuntimeSingletonAttribute>() != null)
                .OrderBy(t => t.GetCustomAttribute<RuntimeSingletonAttribute>()!.Order);

            foreach (var type in types)
                InitializeRuntimeSingleton(type);
        }

        private static void InitializeRuntimeSingleton(Type type)
        {
            var property = type.GetProperty(
                    "Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (property?.GetValue(null) is not MonoBehaviour instance) 
            {
                Debug.LogError(
                    $"RuntimeSingletonBootstrap: " +
                    $"{ type.Name} is marked RuntimeSingleton but failed to provide " +
                    $"a valid Singleton<T> MonoBehaviour instance.");

                return;
            }

            UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
        }

        private static bool IsValidRuntimeSingleton(Type type)
            => IsValidSingleton(type) && typeof(MonoBehaviour).IsAssignableFrom(type);

        private static bool IsValidSingleton(Type type)
        {
            var baseType = type;

            while (baseType != null)
            {
                if (baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(Singleton<>))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        private static MethodInfo FindResetMethod(Type type)
        {
            while (type != null)
            {
                var method = type.GetMethods(
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                    .FirstOrDefault(m => m.GetCustomAttribute<SingletonResetAttribute>() != null);

                if (method != null) return method;

                type = type.BaseType;
            }

            return null;
        }
    }
}