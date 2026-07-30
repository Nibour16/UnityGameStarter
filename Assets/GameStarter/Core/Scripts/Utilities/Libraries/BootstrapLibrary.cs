using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityGameStarter.CommonData;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.BootstrapLibrary
{
    public static class BootstrapLibrary
    {
        public static void Execute<TPhase>(
            List<Initializer<TPhase>> initializers, TPhase phase)
            where TPhase : Enum
        {
            ExecuteInternal(
                phase, initializers.Select(x => (x.phase, x.action)));
        }

        public static void Execute<TPhase, TContext>(
            List<Initializer<TPhase, TContext>> initializers,
            TPhase phase, TContext context)
            where TPhase : Enum where TContext : GameStarterContext
        {
            ExecuteInternal(
                phase, initializers.Select(x => (x.phase, (Action)(() => x.action(context)))));
        }

        private static void ExecuteInternal<TPhase>(
            TPhase targetPhase, IEnumerable<(TPhase phase, Action invoke)> actions)
            where TPhase : Enum
        {
            foreach (var (phase, invoke) in actions)
            {
                if (!EqualityComparer<TPhase>.Default.Equals(phase, targetPhase))
                    continue;

                invoke();
            }
        }

        public static void Discover<TAttribute, TPhase>(
            List<Initializer<TPhase>> initializers, 
            Func<TAttribute, TPhase> phaseSelector,
            Func<TPhase, int> phaseOrder) 
            where TAttribute : OrderedAttribute where TPhase : Enum
        {
            initializers.Clear();

            DiscoverMethods<TAttribute>((method, attribute) =>
            {
                if (method.ReturnType != typeof(void)) return;

                if (method.GetParameters().Length != 0) return;

                try
                {
                    var action = (Action)Delegate.CreateDelegate(
                        typeof(Action), method);

                    initializers.Add(
                        new Initializer<TPhase>(
                            phaseSelector(attribute),
                            attribute.Order,
                            action));
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        $"Failed to create delegate '{method.DeclaringType?.FullName}.{method.Name}'.\n{e}");
                }
            });

            initializers.Sort(phaseOrder);
        }

        public static void Discover<TAttribute, TPhase, TContext>(
            List<Initializer<TPhase, TContext>> initializers,
            Func<TAttribute, TPhase> phaseSelector,
            Func<TPhase, int> phaseOrder)
            where TAttribute : OrderedAttribute
            where TPhase : Enum where TContext : GameStarterContext
        {
            initializers.Clear();

            DiscoverMethods<TAttribute>((method, attribute) =>
            {
                if (method.ReturnType != typeof(void)) return;

                var parameters = method.GetParameters();

                if (parameters.Length != 1) return;

                if (!parameters[0].ParameterType.IsAssignableFrom(typeof(TContext))) return;

                try
                {
                    var action = (Action<TContext>)Delegate.CreateDelegate(
                        typeof(Action<TContext>), method);

                    initializers.Add(
                        new Initializer<TPhase, TContext>(phaseSelector(attribute), attribute.Order, action));
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        $"Failed to create delegate '{method.DeclaringType?.FullName}.{method.Name}'.\n{e}");
                }
            });

            initializers.Sort(phaseOrder);
        }

        private static void DiscoverMethods<TAttribute>(
            Action<MethodInfo, TAttribute> callback)
            where TAttribute : Attribute
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attribute = method.GetCustomAttribute<TAttribute>();

                        if (attribute != null)
                            callback(method, attribute);
                    }
                }
            }
        }

        private static void Sort<T>(
            this List<Initializer<T>> initializers, Func<T, int> phaseOrder) 
            where T : Enum
        {
            initializers.SortInternal(x => x.phase, x => x.order, phaseOrder);
        }

        private static void Sort<T, TContext>(
            this List<Initializer<T, TContext>> initializers, Func<T, int> phaseOrder)
            where T : Enum where TContext : GameStarterContext
        {
            initializers.SortInternal(x => x.phase, x => x.order, phaseOrder);
        }

        private static void SortInternal<TInitializer, TPhase>(
            this List<TInitializer> initializers,
            Func<TInitializer, TPhase> phaseSelector, 
            Func<TInitializer, int> orderSelector,
            Func<TPhase, int> phaseOrder)
            where TPhase : Enum
        {
            initializers.Sort((a, b) =>
            {
                int phase = phaseOrder(phaseSelector(a)).CompareTo(phaseOrder(phaseSelector(b)));
                return phase != 0 ? phase : orderSelector(a).CompareTo(orderSelector(b));
            });
        }
    }
}