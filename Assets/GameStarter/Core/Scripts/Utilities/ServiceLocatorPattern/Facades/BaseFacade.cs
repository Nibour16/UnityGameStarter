using UnityEngine;

namespace UnityGameStarter.ServiceLocatorPattern.FacadeModule
{
    public abstract class BaseFacade<TService, TManager> : MonoBehaviour
        where TService : class, IService where TManager : MonoBehaviour
    {
        private TManager _manager;
        protected TManager Manager => _manager;

        private FacadeModule<TService, TManager> _module;

        protected virtual void Awake()
        {
            _module = new FacadeModule<TService, TManager>(this, out var manager);
            _manager = manager;
        }

        protected virtual void OnDestroy() => _module.UnregisterService();

        #if UNITY_EDITOR
        protected virtual void OnValidate() => _module.IsValidateService();
        #endif
    }
}