using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class GridSnapper
    {
        private readonly IGridSource _source;

        public GridSnapper(IGridSource source) => _source = source;

        public bool TrySnapToGrid(GameObject entity, string layerName, out Vector3Int gridID, 
            out Vector3Int coord, bool setToBlock = true)
        {
            gridID = default;
            coord = default;

            if (_source == null || !_source.IsGridGenerated) return false;

            if (entity == null || !_source.TryGetGridByLayerName(layerName, out Grid grid) || grid == null)
                return false;

            Vector3 entityPos = entity.transform.position;

            entity.transform.position = grid.GetCellCenterWorldPosition(entityPos);
            
            if (!grid.TrySetEntity(entity, out gridID, out coord, setToBlock)) 
            {
                entity.transform.position = entityPos;
                return false;
            }

            if (entity.TryGetComponent<IGridSnappable>(out var snappable))
                snappable.OnSnapped(layerName, gridID, coord);

            return true;
        }
    }
}