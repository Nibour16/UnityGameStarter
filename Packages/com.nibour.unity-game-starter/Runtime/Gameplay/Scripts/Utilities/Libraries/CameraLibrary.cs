using UnityEngine;

namespace UnityGameStarter.Gameplay.Camera 
{
    public static class CameraLibrary
    {
        public static Vector3 ResolveCollision(Vector3 target, Vector3 desired,
            float radius, LayerMask mask)
        {
            Vector3 direction = (desired - target).normalized;

            float distance = Vector3.Distance(desired, target);

            if (Physics.SphereCast(
                target, radius, direction, out RaycastHit hit, distance, mask))
                return hit.point - direction * radius;

            return desired;
        }
    }
}