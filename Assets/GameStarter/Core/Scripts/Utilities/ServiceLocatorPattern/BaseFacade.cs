using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.ServiceLocatorPattern 
{
    public class BaseFacade<TService, TManager> : MonoBehaviour
        where TService : class, IService where TManager : MonoBehaviour
    {
        private TManager _manager;
        protected TManager Manager => _manager;

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            _manager = ResolveManager();

            if (Manager == null)
                Debug.LogError($"{typeof(TManager).Name} could not be resolved on {name}");

            RegisterService();
        }

        protected virtual void OnDestroy() => UnregisterService();

        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (this is not TService)
                Debug.LogError($"{GetType().Name} must implement {typeof(TService).Name}");
        }
        #endif
        #endregion

        #region Manager Resolution
        /// <summary>
        /// Override this if you want custom resolving logic.
        /// Default: Try Singleton, fallback to GetComponent.
        /// </summary>
        protected virtual TManager ResolveManager()
        {
            // Try singleton first (safe, no reflection)
            if (ManagerIsSingleton(out var singletonInstance))
                return singletonInstance;

            // Fallback: same GameObject
            return GetComponent<TManager>();
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
        #endregion

        #region Service Registration
        protected virtual void RegisterService()
        {
            if (this is TService service)
                ServiceLocator.Register(service);
            else
                Debug.LogError($"{GetType().Name} must implement {typeof(TService).Name}");
        }

        protected virtual void UnregisterService() => ServiceLocator.Unregister<TService>();
        #endregion
    }
}