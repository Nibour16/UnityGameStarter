using UnityEngine;

namespace UnityGameStarter.Gameplay.Grid
{
    public enum GridState { Empty, Occupied, Blocked }

    public class GridElement
    {
        public Vector3Int coordinate;
        public GridState state;
    }

    public class SurfaceGridElement : GridElement
    {
        public Vector3 faceEuler;
    }

    public struct Layer 
    {
        public string name;
        public int id;
    }
}