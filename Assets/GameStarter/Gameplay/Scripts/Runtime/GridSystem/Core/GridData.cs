using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public enum GridOriginType  {  Center, LowerLeftCell, LowerLeftCorner }

    public enum CenterOriginMode { Intersection, CellCenter }

    public enum GridState { Empty, Blocked, Invalid }

    public readonly struct GridOriginData
    {
        public readonly Vector3 GridOrigin;
        public readonly Vector3Int GridOriginCoord;
        public readonly Vector3 WorldOrigin;

        public GridOriginData(Vector3 gridOrigin, Vector3Int gridOriginCoord, Vector3 worldOrigin)
        {
            GridOrigin = gridOrigin;
            GridOriginCoord = gridOriginCoord;
            WorldOrigin = worldOrigin;
        }
    }

    public struct GridElement
    {
        public readonly Vector3Int coordinate;
        public readonly Quaternion faceRotator;

        public GridState state;

        public GridElement(
            Vector3Int coordinate, Quaternion faceRotator, GridState state = GridState.Empty) 
        {
            this.coordinate = coordinate;
            this.faceRotator = faceRotator;
            this.state = state;
        }
    }

    public struct Layer 
    {
        public string name;
        public int id;
    }
}