using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameStarter.Gameplay.GridSystem;

public class TilemapGridGenerator : BaseGridGenerator<TilemapSurfaceSampler>
{
    [SerializeField] private Tilemap tilemap;

    protected override TilemapSurfaceSampler Sampler => new(tilemap, Vector3.forward);
    protected override Vector3 SampleDirection => Vector3.back;

    public override Vector3Int CellCount
    {
        get
        {
            BoundsInt bounds = tilemap.cellBounds;

            bool found = false;
            Vector3Int min = default;
            Vector3Int max = default;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos))
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
            BoundsInt bounds = tilemap.cellBounds;

            bool found = false;
            Vector3Int min = default;
            Vector3Int max = default;

            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(cell))
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
                return tilemap.transform.position;

            Vector3 minWorld = tilemap.CellToWorld(min);
            Vector3 maxWorld = tilemap.CellToWorld(max + Vector3Int.one);

            return (minWorld + maxWorld) * 0.5f;
        }
    }

    public override Vector3 GridRight
    {
        get
        {
            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 next = tilemap.CellToWorld(Vector3Int.right);

            return (next - origin).normalized;
        }
    }

    public override Vector3 GridUp
    {
        get
        {
            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 next = tilemap.CellToWorld(Vector3Int.up);

            return (next - origin).normalized;
        }
    }

    public override Vector3 GridForward
    {
        get
        {
            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 next = tilemap.CellToWorld(Vector3Int.forward);

            return (next - origin).normalized;
        }
    }
}