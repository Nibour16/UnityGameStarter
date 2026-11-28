using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    [System.Serializable]
    public class TilemapLayer : LayerConfig
    {
        public Tilemap targetTilemap;
    }

    public class TilemapGridGenerator : BaseGridGenerator<TilemapSurfaceSampler, TilemapLayer>
    {
        protected override Vector3 SampleDirection => -MainGridForward;

        protected override TilemapSurfaceSampler Sampler(Layer layer)
            => new(GetLayerConfig(layer).targetTilemap, MainGridForward);

        public override Vector3Int GetCellCount(Layer layer)
        {
            var tilemap = GetLayerConfig(layer).targetTilemap;
            BoundsInt bounds = tilemap.cellBounds;

            bool found = false;
            Vector3Int min = default;
            Vector3Int max = default;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;

                if (!found)
                {
                    min = max = pos;
                    found = true;
                }
                else
                {
                    min = Vector3Int.Min(min, pos);
                    max = Vector3Int.Max(max, pos);
                }
            }

            return found ? max - min + Vector3Int.one : Vector3Int.zero;
        }

        protected override Vector3 GetGridCenter(Layer layer) 
        {
            var tilemap = GetLayerConfig(layer).targetTilemap;
            BoundsInt bounds = tilemap.cellBounds;

            bool found = false;
            Vector3Int min = default;
            Vector3Int max = default;

            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(cell)) continue;

                if (!found)
                {
                    min = max = cell;
                    found = true;
                }
                else
                {
                    min = Vector3Int.Min(min, cell);
                    max = Vector3Int.Max(max, cell);
                }
            }

            if (!found) return tilemap.transform.position;

            Vector3 minWorld = tilemap.CellToWorld(min);
            Vector3 maxWorld = tilemap.CellToWorld(max + Vector3Int.one);

            return (minWorld + maxWorld) * 0.5f;
        }

        public override Vector3 GetGridRight(Layer layer) 
        {
            var tilemap = GetLayerConfig(layer).targetTilemap;

            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 next = tilemap.CellToWorld(Vector3Int.right);

            return (next - origin).normalized;
        }

        public override Vector3 GetGridUp(Layer layer) 
        {
            var tilemap = GetLayerConfig(layer).targetTilemap;

            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 next = tilemap.CellToWorld(Vector3Int.up);

            return (next - origin).normalized;
        }

        public override Vector3 GetGridForward(Layer layer)
        {
            var tilemap = GetLayerConfig(layer).targetTilemap;

            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 next = tilemap.CellToWorld(Vector3Int.forward);

            return (next - origin).normalized;
        }
    }
}