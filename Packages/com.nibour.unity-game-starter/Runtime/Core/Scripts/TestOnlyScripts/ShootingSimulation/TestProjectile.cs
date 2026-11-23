using UnityEngine;
using UnityGameStarter.Pool;
using UnityGameStarter.TimerSystem;

namespace UnityGameStarter.TestOnlyScripts.ProjectileSimulation 
{
    [RequireComponent(typeof(Rigidbody))]
    public class TestProjectile : MonoBehaviour, IPoolable
    {
        [SerializeField, Min(0f)] private float speed = 10f;
        [SerializeField, Min(0f)] private float remainingTime = 3f;

        public float Speed => speed;

        private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        private Timer _timer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void OnSpawn()
        {
            _timer = TimerManager.Instance.CreateTimer(
                this, remainingTime, "Projectile Timer", TimerType.Default, false);

            _timer.Start(Despawn);
        }

        public void OnDespawn()
        {
            Debug.Log("Test Projectile: Despawned");

            _rb.linearVelocity = Vector3.zero;
            _timer?.Cancel();
        }

        private void Despawn()
        {
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}