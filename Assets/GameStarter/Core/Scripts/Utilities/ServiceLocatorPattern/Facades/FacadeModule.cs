using System.Xml.Linq;
using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.ServiceLocatorPattern.FacadeModule
{
    internal class FacadeModule<TService, TManager> 
        where TService : class, IService where TManager : MonoBehaviour
    {
        private readonly MonoBehaviour _facade;

        public FacadeModule(MonoBehaviour facade, out TManager manager) 
        {
            _facade = facade;
            IsValidateService();
            manager = ResolveManager();
            RegisterService();
        }

        private TManager ResolveManager()
        {
            TManager manager;
            
            // Try singleton first (safe, no reflection)
            if (ManagerIsSingleton(out var singletonInstance))
                manager = singletonInstance;

            // Fallback: same GameObject
            manager = _facade.GetComponent<TManager>();

            if (manager == null)
                Debug.LogError($"{typeof(TManager).Name} could not be resolved on {_facade.name}");

            return manager;
        }

        /// <summary>
        /// Try resolve as Singleton if TManager inherits Singleton&lt;TManager&gt;
        /// </summary>
        private bool ManagerIsSingleton(out TManager instance)
        {
            instance = null;

            if (typeof(Singleton<TManager>).IsAssignableFrom(typeof(TManager)))
            {
                instance = Singleton<TManager>.Instance;
                return instance != null;
            }

            return false;
        }

        private void RegisterService()
        {
            if (_facade is TService service)
                ServiceLocator.Register(service);
            else
                Debug.LogError($"{_facade.GetType().Name} must implement {typeof(TService).Name}");
        }

        public void UnregisterService() => ServiceLocator.Unregister<TService>();

        public bool IsValidateService() 
        {
            if (_facade is not TService) 
            {
                Debug.LogError($"{_facade.GetType().Name} must implement {typeof(TService).Name}");
                return false;
            }

            return true;
        }
    }
}