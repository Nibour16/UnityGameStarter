using System;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;

using UnityGameStarter.RuntimeCore;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RuntimeSingletonAttribute : OrderedAttribute 
    {
        public RuntimeSingletonAttribute(int order = 0) : base(order) { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class SingletonResetAttribute : Attribute { }

    public static class RuntimeSingletonBootstrap
    {
        [GameStarter(RuntimeInitializeLoadType.SubsystemRegistration, -290)]
        public static void Reset()
        {
            var types = TypeCache.GetTypesWithAttribute<RuntimeSingletonAttribute>().Where(t => !t.IsAbstract);

            foreach (var type in types)
            {
                var method = FindResetMethod(type);

                method?.Invoke(null, null);
            }
        }

        [GameStarter(RuntimeInitializeLoadType.BeforeSceneLoad, -290)]
        public static void Initialize()
        {
            var types = TypeCache
                .GetTypesWithAttribute<RuntimeSingletonAttribute>()
                .Where(t => !t.IsAbstract && IsRuntimeSingleton(t))
                .OrderBy(t => t.GetCustomAttribute<RuntimeSingletonAttribute>().Order);

            foreach (var type in types)
            {
                var property = type.GetProperty(
                    "Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

                property?.GetValue(null);
            }
        }

        private static bool IsRuntimeSingleton(Type type)
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
            var singletonType = type.BaseType;

            if (singletonType == null || !IsRuntimeSingleton(type))
                return null;

            var methods = singletonType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
            return methods?.FirstOrDefault(m => m.GetCustomAttribute<SingletonResetAttribute>() != null);
        }
    }
}