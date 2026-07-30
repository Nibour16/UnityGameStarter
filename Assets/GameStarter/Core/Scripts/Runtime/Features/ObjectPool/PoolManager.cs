using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.Pool.Internal;

namespace UnityGameStarter.Pool
{
    [RuntimeSingleton]
    public class PoolManager : Singleton<PoolManager>
    {
        private readonly Dictionary<GameObject, GameObjectPool> _pools = new();
        private readonly Dictionary<GameObject, GameObjectPool> _activeInstances = new();

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                pool = new GameObjectPool(prefab);
                _pools.Add(prefab, pool);
            }

            var instance = pool.Spawn(position, rotation);
            _activeInstances.Add(instance, pool);
            return instance;
        }

        public void Despawn(GameObject instance)
        {
            if (instance == null) return;

            if (_activeInstances.Remove(instance, out var pool))
                pool.Release(instance);
            else
                Destroy(instance);
        }

        public void Destroy(GameObject instance)
        {
            _activeInstances.Remove(instance);

            Object.Destroy(instance);
        }
    }
}