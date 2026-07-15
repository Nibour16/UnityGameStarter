using System;
using UnityEngine;
using UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap;

namespace UnityGameStarter.SingletonPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isQuitting = false;

        public static T Instance
        {
            get
            {
                if (_isQuitting || !Application.isPlaying) return null;

                if (_instance == null) CreateInstance();

                return _instance;
            }
        }

        protected static bool IsValidSingletonType(Type type) => typeof(T) == type;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
                Destroy(gameObject);
        }

        private static void CreateInstance()
        {
            if (_isQuitting || !Application.isPlaying) return;

            _instance = FindAnyObjectByType<T>();

            if (_instance != null) return;

            var obj = new GameObject(typeof(T).Name);
            _instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
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
            if (_instance == this)
                _instance = null;
        }
    }
}