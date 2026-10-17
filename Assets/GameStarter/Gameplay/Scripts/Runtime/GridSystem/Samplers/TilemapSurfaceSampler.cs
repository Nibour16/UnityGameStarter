using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public sealed class TilemapSurfaceSampler : ISurfaceSampler
    {
        private readonly Tilemap _tilemap;
        private readonly Plane _plane;

        public TilemapSurfaceSampler(Tilemap tilemap, Vector3 normal)
        {
            _tilemap = tilemap;
            _plane = new Plane(normal.normalized, tilemap.transform.position);
        }

        public bool TrySample(Vector3 origin, Vector3 direction, float distance, out SurfaceSample sample)
        {
            sample = default;

            if (_tilemap == null) return false;

            Vector3 hitPoint;

            if (Mathf.Approximately(_plane.GetDistanceToPoint(origin), 0f))
                hitPoint = origin;
            else
            {
                Ray ray = new(origin, direction);

                if (!_plane.Raycast(ray, out float enter) || enter < 0f || enter > distance)
                    return false;

                hitPoint = ray.GetPoint(enter);
            }

            if (!_tilemap.HasTile(_tilemap.WorldToCell(hitPoint))) return false;

            sample = new SurfaceSample(hitPoint, _plane.normal);
            return true;
        }
    }
}