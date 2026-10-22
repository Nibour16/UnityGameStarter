using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class GridSnapper
    {
        private readonly IGridSource _source;

        public GridSnapper(IGridSource source) => _source = source;

        public bool TrySnapToGrid(GameObject entity, string layerName, 
            out Vector3Int coord, bool setToBlock = true)
        {
            coord = default;

            if (_source == null || !_source.IsGridGenerated) return false;
            
            Layer layer = _source.GetLayerByName(layerName);

            if (entity == null || !_source.Grids.TryGetValue(layer, out Grid grid) || grid == null)
                return false;

            Vector3 entityPos = entity.transform.position;

            entity.transform.position = grid.GetCellCenterWorldPosition(entityPos);

            if (!grid.TrySetEntity(entity, out coord, setToBlock)) 
            {
                entity.transform.position = entityPos;
                return false;
            }

            if (entity.TryGetComponent<IGridSnappable>(out var snappable))
                snappable.OnSnapped(layerName, coord);

            return true;
        }
    }
}