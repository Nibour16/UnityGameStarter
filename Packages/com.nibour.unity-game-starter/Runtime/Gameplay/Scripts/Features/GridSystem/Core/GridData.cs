using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public enum GridOriginType  {  Center, LowerLeftCell, LowerLeftCorner }

    public enum CenterOriginMode { Intersection, CellCenter }

    public enum GridState { Passable, Occupied, Blocked, Invalid }

    public readonly struct GridOriginData
    {
        public readonly Vector3 gridOrigin;
        public readonly Vector3Int gridOriginCoord;
        public readonly Vector3 worldOrigin;

        public GridOriginData(Vector3 gridOrigin, Vector3Int gridOriginCoord, Vector3 worldOrigin)
        {
            this.gridOrigin = gridOrigin;
            this.gridOriginCoord = gridOriginCoord;
            this.worldOrigin = worldOrigin;
        }
    }

    public struct GridElement
    {
        public readonly Vector3Int coordinate;
        public readonly Quaternion faceRotator;

        private readonly HashSet<GameObject> _entities;

        private readonly GridState _originalState;
        private GridState _state;
        public GridState State 
        {
            get 
            {
                if (Entities.Count <= 0 && _state == GridState.Occupied)
                    _state = _originalState;

                return _state;
            }
            set => _state = value;
        }

        public readonly HashSet<GameObject> Entities 
        {
            get 
            {
                _entities.RemoveWhere(entity => entity == null);
                return _entities;
            }
        }

        public GridElement(
            Vector3Int coordinate, Quaternion faceRotator, 
            GridState state = GridState.Passable, GameObject[] entities = null) 
        {
            this.coordinate = coordinate;
            this.faceRotator = faceRotator;
            _state = state;
            _originalState = _state;

            _entities = new HashSet<GameObject>(entities ?? System.Array.Empty<GameObject>());
        }
    }

    public struct Layer 
    {
        public string name;
        public int id;
    }
}