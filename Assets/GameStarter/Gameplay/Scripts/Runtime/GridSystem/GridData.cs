using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public enum GridOriginType  {  Center, LowerLeftCell, LowerLeftCorner }

    public enum CenterOriginMode { Intersection, CellCenter }

    public enum GridState { Empty, Blocked, Invalid }

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