using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RuntimeSingletonAttribute : Attribute { }

    public static class RuntimeSingletonBootstrap
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var types = TypeCache.GetTypesWithAttribute<RuntimeSingletonAttribute>().Where(t => !t.IsAbstract);

            foreach (var type in types)
            {
                var property = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);

                property?.GetValue(null);
            }
        }
    }
}