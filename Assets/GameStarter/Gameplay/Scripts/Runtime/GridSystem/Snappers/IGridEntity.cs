using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public interface IGridEntity { }
    
    public interface IGridSnappable : IGridEntity
    {
        void OnSnapped(string snappedLayer, Vector3Int gridID, Vector3Int coord);
    }

    public interface IGridCollideEvent : IGridEntity
    {
        void OnHit(GameObject other);
    }
}