using UnityEngine;
using UnityGameStarter.LoopHelper;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Math.Grid 
{
    public static class GridLibrary
    {
        public static bool TryWorldToGrid(
            this Vector3Int position, Vector3Int[,,] grid, Vector3Int origin, out Vector3Int gridCoord)
        {
            gridCoord = position;

            int x = position.x - origin.x;
            int y = position.y - origin.y;
            int z = position.z - origin.z;

            if (x < 0 || y < 0 || z < 0) return false;

            if (x >= grid.GetLength(0) || y >= grid.GetLength(1) || z >= grid.GetLength(2)) return false;

            gridCoord = grid[x, y, z];
            return true;
        }

        public static bool TryWorldToGrid(
            this Vector3 position, Vector3Int[,,] grid, Vector3Int origin, out Vector3Int gridCoord)
            => position.ToVector3Int().TryWorldToGrid(grid, origin, out gridCoord);

        public static Vector3 GridToWorld(this Vector3Int gridCoord, Vector3Int origin)
        {
            int x = gridCoord.x + origin.x;
            int y = gridCoord.y + origin.y;
            int z = gridCoord.z + origin.z;

            return new Vector3(x, y, z);
        }

        public static Vector3Int GridToWorldInt(this Vector3Int gridCoord, Vector3Int origin)
            => gridCoord.GridToWorld(origin).ToVector3Int();

        public static Vector3Int[,,] GenerateGridData(Vector3Int size, Vector3Int origin) 
        {
            var grid = new Vector3Int[size.x, size.y, size.z];

            LoopLibrary.Loop3D(size.x, size.y, size.z, (x, y, z) => 
                grid[x, y, z] = new Vector3Int(x + origin.x, y + origin.y, z + origin.z));      

            return grid;
        }
    }
}