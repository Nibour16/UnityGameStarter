using UnityEngine;
using UnityEngine.Pool;

namespace UnityGameStarter.Pool
{
    /// <summary>
    /// Pool data that manipulates Unity's Pool to simulate pooling to make it more suitable for an game object
    /// </summary>
    class GameObjectPool
    {
        public GameObject Prefab { get;}

        private readonly ObjectPool<GameObject> _pool;

        public GameObjectPool(GameObject prefab)
        {
            Prefab = prefab;

            _pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease, OnDestroy);
        }

        #region API
        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            var obj = _pool.Get();
            obj.transform.SetPositionAndRotation(pos, rot);
            return obj;
        }

        public void Release(GameObject obj) => _pool.Release(obj);

        public void Destroy(GameObject obj) => OnDestroy(obj);

        public void Clear() => _pool.Clear();
        #endregion

        #region Methods that applied to Unity Pool Class
        private GameObject Create() => Object.Instantiate(Prefab);

        private void OnGet(GameObject obj)
        {
            obj.SetActive(true);

            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnSpawn();
        }

        private void OnRelease(GameObject obj)
        {
            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnDespawn();

            obj.SetActive(false);
        }

        private void OnDestroy(GameObject obj) 
        {
            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnDespawn();

            Object.Destroy(obj);
        }
        #endregion
    }
}