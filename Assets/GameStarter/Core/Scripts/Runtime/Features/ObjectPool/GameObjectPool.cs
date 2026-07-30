using UnityEngine;
using UnityEngine.Pool;

namespace UnityGameStarter.Pool.Internal
{
    internal class GameObjectPool
    {
        public GameObject Prefab { get;}

        private readonly ObjectPool<GameObject> _pool;

        public GameObjectPool(GameObject prefab)
        {
            Prefab = prefab;

            _pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease, OnDestroy);
        }

        private GameObject Create() => Object.Instantiate(Prefab);

        private void OnGet(GameObject obj)
        {
            obj.SetActive(true);

            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnSpawn();
        }

        private void OnRelease(GameObject obj)
        {
            obj.SetActive(false);

            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnDespawn();
        }

        private void OnDestroy(GameObject obj) => Object.Destroy(obj);

        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            var obj = _pool.Get();
            obj.transform.SetPositionAndRotation(pos, rot);
            return obj;
        }

        public void Release(GameObject obj) => _pool.Release(obj);
    }
}