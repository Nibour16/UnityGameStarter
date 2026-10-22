using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public interface IGridSnappable
    {
        void OnSnapped(string snappedLayer, Vector3Int coord);
    }
}