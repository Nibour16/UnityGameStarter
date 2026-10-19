using System;
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
        private readonly Vector3 _worldCenter;
        private readonly float _cellSize = 1f;
        private readonly IntConversionMode _gridSizeConversionMode = IntConversionMode.Floor;

        private readonly GridOriginType _originType;
        private readonly CenterOriginMode _centerOriginMode;

        private GridElement[,,] _grid;
        public GridElement[,,] GridData => _grid;

        public Grid(
            Bounds bounds, Vector3 worldCenter, float cellSize,
            ISurfaceSampler sampler, Vector3 sampleDirection, Vector3 cellFaceDirection,
            GridState initialState = GridState.Empty,
            bool includeIncompleteCell = false,
            GridOriginType originType = GridOriginType.Center, 
            CenterOriginMode centerOriginMode = CenterOriginMode.Intersection) 
        {
            _bounds = bounds;
            _worldCenter = worldCenter;
            _cellSize = cellSize;
            _gridSizeConversionMode = includeIncompleteCell ? IntConversionMode.Ceil : IntConversionMode.Floor;

            _originType = originType;
            _centerOriginMode = centerOriginMode;

            GenerateGrid(sampler, sampleDirection, cellFaceDirection, initialState);
        }

        public Vector3Int GetGridSize() 
        {
            int x = StandardMathLibrary.IntConversion(_bounds.size.x / _cellSize, _gridSizeConversionMode);
            int y = StandardMathLibrary.IntConversion(_bounds.size.y / _cellSize, _gridSizeConversionMode);
            int z = StandardMathLibrary.IntConversion(_bounds.size.z / _cellSize, _gridSizeConversionMode);

            return new Vector3Int(x, y, z);
        }

        public Vector3 GetGridOrigin(
            GridOriginType originType, CenterOriginMode centerMode = CenterOriginMode.Intersection)
        {
            Vector3Int size = GetGridSize();

            return originType switch
            {
                GridOriginType.Center => -GetCenterOriginOffset(size, centerMode),
                GridOriginType.LowerLeftCell or GridOriginType.LowerLeftCorner => Vector3.zero,
                _ => throw new ArgumentOutOfRangeException(nameof(originType), originType, null)
            };
        }

        public Vector3Int GetGridOriginCoord(
            GridOriginType originType, CenterOriginMode centerMode = CenterOriginMode.Intersection)
            => GetGridOrigin(originType, centerMode).ToGridCoordinate();

        private Vector3 GetCenterOriginOffset(Vector3Int size, CenterOriginMode centerMode)
        {
            Vector3 sizeOffset = (Vector3)size;

            return centerMode switch
            {
                CenterOriginMode.CellCenter => new Vector3(size.x / 2, size.y / 2, size.z / 2),

                CenterOriginMode.Intersection =>
                    (sizeOffset - Vector3.one) * 0.5f,

                _ => throw new ArgumentOutOfRangeException(
                    nameof(centerMode), centerMode, null)
            };
        }

        public Vector3 GetOriginWorldPosition(
            GridOriginType originType, CenterOriginMode centerMode = CenterOriginMode.Intersection)
        {
            Vector3Int size = GetGridSize();

            switch (originType)
            {
                case GridOriginType.Center:
                {
                    if (centerMode == CenterOriginMode.Intersection)
                        return _worldCenter;

                    Vector3Int centerCell = new(size.x / 2, size.y / 2, size.z / 2);

                    Vector3 centerOffset =
                        (Vector3)centerCell - ((Vector3)size - Vector3.one) * 0.5f;

                    var result = _worldCenter + centerOffset * _cellSize;
                    return result;
                }

                case GridOriginType.LowerLeftCell:
                    return _worldCenter - _cellSize * 0.5f * ((Vector3)size - Vector3.one);

                case GridOriginType.LowerLeftCorner:
                    return _worldCenter - _cellSize * 0.5f * (Vector3)size;

                default:
                    throw new ArgumentOutOfRangeException(nameof(originType), originType, null);
            }
        }

        public Vector3 GetCellWorldPosition(Vector3Int localCoord, out Vector3Int relativeCoord,
            GridOriginType originType, CenterOriginMode centerMode = CenterOriginMode.Intersection)
        {
            Vector3 originPosition = GetOriginWorldPosition(originType, centerMode);
            Vector3 gridOrigin = GetGridOrigin(originType, centerMode);

            Vector3Int originCoord = GetGridOriginCoord(originType, centerMode);
            relativeCoord = localCoord + originCoord;

            Vector3 worldPosition = relativeCoord.GridToWorld(
                originPosition, gridOrigin, originCoord, _cellSize);

            if (originType == GridOriginType.LowerLeftCorner)
                worldPosition += Vector3.one * (_cellSize * 0.5f);

            return worldPosition;
        }

        private void GenerateGrid(ISurfaceSampler sampler, Vector3 sampleDirection, Vector3 cellFaceDirection,
            GridState defaultState = GridState.Empty) 
        {
            Vector3Int size = GetGridSize();
            _grid = new GridElement[size.x, size.y, size.z];

            LoopLibrary.Loop3D(size.x, size.y, size.z, (x, y, z) =>
            {
                Vector3Int localCoord = new(x, y, z);
                Vector3 worldPosition = GetCellWorldPosition(
                    localCoord, out var relativeCoord, _originType, _centerOriginMode);

                if (sampler.TrySample(worldPosition, sampleDirection, _cellSize, out var sample))
                {
                    var rotator = defaultState switch
                    {
                        GridState.Invalid => Quaternion.identity,
                        _ => Quaternion.FromToRotation(cellFaceDirection, sample.normal)
                    };

                    _grid[x, y, z] = new GridElement(relativeCoord, rotator, defaultState);
                }
                else
                    _grid[x, y, z] = new GridElement(relativeCoord, Quaternion.identity, GridState.Invalid);
            });
        }
    }
}