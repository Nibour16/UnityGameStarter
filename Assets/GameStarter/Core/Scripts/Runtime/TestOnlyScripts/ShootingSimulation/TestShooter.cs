using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.Pool;

namespace UnityGameStarter.TestOnlyScripts.ProjectileSimulation 
{
    public class TestShooter : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform shootingTrans;

        private void Update() 
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame) 
            {
                var proj = PoolManager.Instance.Spawn(projectile, shootingTrans.position, transform.rotation);

                if (proj.TryGetComponent<TestProjectile>(out var component))
                    component.Rb.linearVelocity = transform.forward * component.Speed;
            }
        }
    }
}