using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem
{
    public enum GridState { Empty, Occupied, Blocked }

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