using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public enum GridOriginType  {  Center, LowerLeftCell, LowerLeftCorner }

    public enum CenterOriginMode { Intersection, CellCenter }

    public enum GridState { Passable, Blocked, Invalid }

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

        public GridState state;
        public HashSet<GameObject> entities;

        public GridElement(
            Vector3Int coordinate, Quaternion faceRotator, 
            GridState state = GridState.Passable, GameObject[] entities = null) 
        {
            this.coordinate = coordinate;
            this.faceRotator = faceRotator;
            this.state = state;

            this.entities = new HashSet<GameObject>(entities ?? System.Array.Empty<GameObject>());
        }
    }

    public struct Layer 
    {
        public string name;
        public int id;
    }
}