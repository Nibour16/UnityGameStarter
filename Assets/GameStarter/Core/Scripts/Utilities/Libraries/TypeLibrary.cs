using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGameStarter.TypeLibrary 
{
    public static class TypeLibrary
    {
        public static bool TryCast<T>(object obj, out T result, bool printInvalid = true) where T : class
        {
            if (obj is T t) 
            {
                result = t;
                return true;
            }

            result = null;

            if (printInvalid)
                Debug.LogWarning($"TypeLibrary: cast failed, input object is not valid type of {typeof(T)}");
            
            return false;
        }

        public static bool IsSubclassOf<TBase>(Type type, bool printError = true)
        {
            if (type == null)
            {
                if (printError)
                    Debug.LogError("TypeLibrary: Invalid type detected!");

                return false;
            }

            if (type.IsInterface || type.IsAbstract)
            {
                if (printError)
                    Debug.LogError($"TypeLibrary: {type.Name} is interface or abstract");
                return false;
            }

            if (!typeof(TBase).IsAssignableFrom(type))
            {
                if (printError)
                    Debug.LogError($"TypeLibrary: {type.Name} is not a {typeof(TBase).Name}");
                return false;
            }

            return true;
        }

        public static IEnumerable<Type> GetValidSubTypes<TBase>()
        {
            // return all valid child types of the base type in the project

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch { return Array.Empty<Type>(); }
            }).Where(t => IsSubclassOf<TBase>(t));
        }

        public static object CreateInstance(Type type, params object[] args)
        {
            if (!IsSubclassOf<object>(type, false))
                throw new Exception($"TypeLibrary: {type.Name} is not valid");

            try { return Activator.CreateInstance(type, args); }

            catch (Exception e)
            {
                string provided = string.Join(", ",
                args.Select(a => a?.GetType().Name ?? "null"));

                throw new InvalidOperationException(
                    $"TypeLibrary: Cannot create '{type.FullName}'. " +
                    $"Arguments: ({provided})", e);
            }
        }

        public static T CreateInstance<T>(Type type, params object[] args)
            => (T)CreateInstance(type, args);
    }
}