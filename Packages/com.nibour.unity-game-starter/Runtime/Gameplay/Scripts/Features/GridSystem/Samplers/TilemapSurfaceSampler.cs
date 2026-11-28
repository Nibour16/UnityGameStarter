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

        public bool TrySample(Vector3 gridPos, Vector3 direction, float distance, out SurfaceSample sample)
        {
            sample = default;

            if (_tilemap == null) return false;

            Vector3 hitPoint;

            if (Mathf.Approximately(_plane.GetDistanceToPoint(gridPos), 0f))
                hitPoint = gridPos;
            else
            {
                Ray ray = new(gridPos, direction);

                bool didHit = _plane.Raycast(ray, out float enter);

                if (!didHit || enter < 0f || enter > distance)
                    return false;

                hitPoint = ray.GetPoint(enter);
            }

            Vector3Int cell = _tilemap.WorldToCell(hitPoint);

            if (!_tilemap.HasTile(cell)) return false;

            sample = new SurfaceSample(hitPoint, _plane.normal);
            return true;
        }
    }
}