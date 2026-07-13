using System;
using System.Collections.Generic;

namespace UnityGameStarter.ServiceLocatorPattern
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service) where T : class, IService
        {
            var type = typeof(T);

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _services[type] = service;
        }

        public static void Unregister<T>() where T : class, IService
        {
            _services.Remove(typeof(T));
        }

        public static bool IsRegistered<T>() where T : class, IService
        {
            return _services.ContainsKey(typeof(T));
        }

        public static T Get<T>() where T : class, IService
        {
            var type = typeof(T);

            if (!_services.TryGetValue(type, out var service))
                throw new Exception($"{typeof(T).Name} not registered.");

            return (T)service;
        }
        
        public static void ClearAll()
        {
            _services.Clear();
        }
    }
}