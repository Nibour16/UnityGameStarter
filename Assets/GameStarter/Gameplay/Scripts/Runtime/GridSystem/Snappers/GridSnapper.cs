using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class GridSnapper : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour source;

        private ISnappableGridSource Source => source as ISnappableGridSource;

        public void SnapToGrid(GameObject entity, Layer layer, bool setToBlock = true)
        {
            if (entity == null || !Source.Grids.TryGetValue(layer, out Grid grid) || grid == null)
                return;

            Vector3 entityPos = entity.transform.position;

            entity.transform.position = grid.GetCellCenterWorldPosition(entityPos);

            grid.TrySetEntity(entity, setToBlock);
        }
    }
}