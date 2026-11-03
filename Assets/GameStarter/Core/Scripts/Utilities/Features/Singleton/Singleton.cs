using System;
using UnityEngine;

namespace UnityGameStarter.SingletonPattern
{
    public interface ISingleton { }

    public class Singleton<T> : MonoBehaviour, ISingleton where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isQuitting = false;

        protected virtual MonoBehaviour InstanceRef => this;
        protected virtual bool DestroyGameObjectOnDuplicate => true;

        public static bool IsQuitting => _isQuitting;

        public static T Instance
        {
            get
            {
                if (_instance == null) CreateInstance();

                return _instance;
            }
        }

        protected static bool IsValidSingletonType(Type type) => typeof(T) == type;

        protected virtual void Awake()
        {
            var instance = InstanceRef;

            if (instance == null) return;

            if (_instance == null)
                _instance = instance as T;
            else if (_instance != instance)
            {
                if (DestroyGameObjectOnDuplicate)
                    Destroy(instance.gameObject);
                else
                    Destroy(instance);
            }
        }

        protected void EnableDontDestroyOnLoad() 
        {
            var instance = InstanceRef;

            if (instance == null || Instance != instance)
                return;

            DontDestroyOnLoad(instance.gameObject);
        }

        public static bool TryGetInstance(out T instance, bool printWarningIfFailed = false)
        {
            instance = _instance;

            if (instance != null) return true;

            instance = FindInstance();

            if (instance == null) 
            {
                if (printWarningIfFailed)
                    Debug.LogWarning($"The Singleton '{typeof(T)}' is not found");

                return false;
            }
            
            _instance = instance;
            return true;
        }

        private static void CreateInstance()
        {
            if (_isQuitting || !Application.isPlaying) return;

            _instance = FindInstance();

            if (_instance != null) return;

            var obj = new GameObject(typeof(T).Name);
            var component = obj.AddComponent<T>();

            if (component is Singleton<T> singleton)
                _instance = singleton.InstanceRef as T;
        }

        private static T FindInstance()
        {
            foreach (var component in FindObjectsByType<T>())
            {
                if (component is not Singleton<T> singleton) continue;

                if (singleton.InstanceRef is T instance) return instance;
            }

            return null;
        }

        [SingletonReset]
        private static void ResetStatics()
        {
            _instance = null;
            _isQuitting = false;
        }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == InstanceRef)
                _instance = null;
        }
    }
}