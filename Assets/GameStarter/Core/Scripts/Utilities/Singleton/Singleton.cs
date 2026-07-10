using UnityEngine;

namespace UnityGameStarter.SingletonPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    CreateInstance();

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)(object)this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
                Destroy(gameObject);
        }

        private static void CreateInstance()
        {
            if (_instance != null) return;

            var obj = new GameObject(typeof(T).Name);
            _instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}