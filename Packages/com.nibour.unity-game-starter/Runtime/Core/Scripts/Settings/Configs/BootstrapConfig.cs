using UnityEngine;

namespace UnityGameStarter.Config 
{
    public abstract class BootstrapConfig<T> : ScriptableObject
    {
        [SerializeField, HideInInspector] private T config;
        public T Config => config;

        #if UNITY_EDITOR
        public void SetConfig(T config)
        {
            this.config = config;
        }
        #endif
    }
}