using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    [System.Serializable]
    public class TilemapLayer : SerializedLayer
    {
        public Tilemap targetTilemap;
    }

    public class TilemapGridGenerator : BaseGridGenerator<TilemapSurfaceSampler, TilemapLayer>
    {
        [SerializeField] private Tilemap mainTilemap;

        private Tilemap MainTilemap
        {
            get
            {
                if (layers.Length <= 0)
                    throw new System.IndexOutOfRangeException("The layer data is not valid");

                if (mainTilemap)
                {
                    foreach (var layer in layers)
                    {
                        if (layer.targetTilemap == mainTilemap)
                            return mainTilemap;
                    }

                    Debug.LogWarning(
                        $"Main Tilemap {mainTilemap} does not belong to one of the layers, " +
                        $"which is not valid, will return the one from the first layer");
                }

                return layers[0].targetTilemap;
            }
        }

        protected override TilemapSurfaceSampler Sampler => new(MainTilemap, Vector3.forward);
        protected override Vector3 SampleDirection => Vector3.back;

        public override Vector3Int CellCount
        {
            get
            {
                BoundsInt bounds = MainTilemap.cellBounds;

                bool found = false;
                Vector3Int min = default;
                Vector3Int max = default;

                foreach (Vector3Int pos in bounds.allPositionsWithin)
                {
                    if (!MainTilemap.HasTile(pos))
                        continue;

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
        }

        protected override Vector3 GridCenter
        {
            get
            {
                BoundsInt bounds = MainTilemap.cellBounds;

                bool found = false;
                Vector3Int min = default;
                Vector3Int max = default;

                foreach (Vector3Int cell in bounds.allPositionsWithin)
                {
                    if (!MainTilemap.HasTile(cell))
                        continue;

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

                if (!found)
                    return MainTilemap.transform.position;

                Vector3 minWorld = MainTilemap.CellToWorld(min);
                Vector3 maxWorld = MainTilemap.CellToWorld(max + Vector3Int.one);

                return (minWorld + maxWorld) * 0.5f;
            }
        }

        public override Vector3 GridRight
        {
            get
            {
                Vector3 origin = MainTilemap.CellToWorld(Vector3Int.zero);
                Vector3 next = MainTilemap.CellToWorld(Vector3Int.right);

                return (next - origin).normalized;
            }
        }

        public override Vector3 GridUp
        {
            get
            {
                Vector3 origin = MainTilemap.CellToWorld(Vector3Int.zero);
                Vector3 next = MainTilemap.CellToWorld(Vector3Int.up);

                return (next - origin).normalized;
            }
        }

        public override Vector3 GridForward
        {
            get
            {
                Vector3 origin = MainTilemap.CellToWorld(Vector3Int.zero);
                Vector3 next = MainTilemap.CellToWorld(Vector3Int.forward);

                return (next - origin).normalized;
            }
        }
    }
}