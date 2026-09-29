using UnityEngine;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public sealed class PlaneSurfaceSampler : ISurfaceSampler
    {
        private readonly Plane _plane;

        public PlaneSurfaceSampler(Vector3 normal, Vector3 point)
        {
            _plane = new Plane(normal, point);
        }

        public bool TrySample(Vector3 origin, Vector3 direction, float distance, out SurfaceSample sample)
        {
            sample = default;
            Ray ray = new(origin, direction);

            if (!_plane.Raycast(ray, out float enter))
                return false;

            if (enter < 0f || enter > distance)
                return false;

            sample = new SurfaceSample(ray.GetPoint(enter), _plane.normal);
            return true;
        }
    }
}