using UnityEngine;
using UnityGameStarter.LoopHelper;
using UnityGameStarter.Math;
using UnityGameStarter.Math.GridStatics;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public class Grid
    {
        private Bounds _bounds;
        private Vector3Int _origin;
        private readonly float _cellSize = 1f;
        private readonly IntConversionMode _gridSizeConversionMode = IntConversionMode.Floor;

        private GridElement[,,] _grid;

        public Grid(
            Bounds bounds, Vector3Int origin, float cellSize, ISurfaceSampler sampler, 
            bool includeIncompleteCell = false) 
        {
            _bounds = bounds;
            _origin = origin;
            _cellSize = cellSize;
            _gridSizeConversionMode = includeIncompleteCell ? IntConversionMode.Ceil : IntConversionMode.Floor;

            GenerateGrid(sampler);
        }

        public Vector3Int GetGridSize() 
        {
            int x = StandardMathLibrary.IntConversion(_bounds.size.x / _cellSize, _gridSizeConversionMode);
            int y = StandardMathLibrary.IntConversion(_bounds.size.y / _cellSize, _gridSizeConversionMode);
            int z = StandardMathLibrary.IntConversion(_bounds.size.z / _cellSize, _gridSizeConversionMode);

            return new Vector3Int(x, y, z);
        }

        private void GenerateGrid(ISurfaceSampler sampler) 
        {
            Vector3Int size = GetGridSize();
            _grid = new GridElement[size.x, size.y, size.z]; 

            LoopLibrary.Loop3D(size.x, size.y, size.z, (x, y, z) =>
            {
                Vector3Int coordinate = new Vector3Int(x, y, z) + _origin;

                Vector3 worldPosition = GridLibrary.GridToWorld(coordinate, _origin);

                if (sampler.TrySample(worldPosition, Vector3.up, _cellSize, out var sample)) 
                {
                    _grid[x, y, z] = new GridElement(
                        coordinate, Quaternion.FromToRotation(Vector3.up, sample.normal), 
                        GridState.Empty);
                }
            });
        }
    }
}