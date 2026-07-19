using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.ServiceLocatorPattern.FacadeModule
{
    public abstract class BaseSingletonFacade<TSingleton, TService, TManager> : Singleton<TSingleton>
        where TSingleton : MonoBehaviour 
        where TService : class, IService 
        where TManager : MonoBehaviour
    {
        private TManager _manager;
        protected TManager Manager => _manager;

        private FacadeModule<TService, TManager> _module;

        protected override void Awake()
        {
            base.Awake();

            _module = new FacadeModule<TService, TManager>(this, out var manager);
            _manager = manager;
        }

        protected override void OnDestroy()
        {
            _module.UnregisterService();
            base.OnDestroy();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate() => _module.IsValidateService();
#endif
    }
}