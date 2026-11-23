using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    [System.Serializable]
    public class SnappableConfig
    {
        public GameObject entity;
        public string targetLayer = "Default";
        public bool setToBlock = true;
    }

    public class SnappableGridManager : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour source;

        [Space]
        [SerializeField] private SnappableConfig[] specialSnappables;

        [Space]
        [SerializeField] private string defaultLayer = "Default";
        [SerializeField] private bool defaultSetToBlockWhenSnapping = true;

        public IGridSource Source => source as IGridSource;

        #region Life Cycle
        private void Start() => SnapAllEntities();

        /// <summary>
        /// Only entities with IGridEntity interface component can be snapped during initialization
        /// </summary>
        private void SnapAllEntities()
        {
            if (!Source.IsGridGenerated) return;
            
            var entities = FindObjectsByType<GameObject>();
            string targetLayer = defaultLayer;
            bool setToBlock = defaultSetToBlockWhenSnapping;

            foreach(var entity in entities) 
            {
                if (!entity.TryGetComponent<IGridEntity>(out _))
                    continue;
                
                if (IsSpecialSnappableEntity(entity, out var config)) 
                {
                    targetLayer = config.targetLayer;
                    setToBlock = config.setToBlock;
                }

                TrySnapToGrid(entity, targetLayer, out _, out _, setToBlock);
            }
        }
        #endregion

        public bool TrySnapToGrid(GameObject entity, string layerName, out Vector3Int gridID,
            out Vector3Int coord, bool setToBlock = true)
        {
            gridID = default;
            coord = default;

            if (Source == null || !Source.IsGridGenerated) return false;

            if (entity == null || !Source.TryGetGridByLayerName(layerName, out Grid grid) || grid == null)
                return false;

            Vector3 entityPos = entity.transform.position;

            entity.transform.position = grid.GetCellCenterWorldPosition(entityPos);

            if (!grid.TrySetEntity(entity, out gridID, out coord, setToBlock))
            {
                entity.transform.position = entityPos;
                return false;
            }

            foreach (var snappable in entity.GetComponents<IGridSnappable>())
                snappable.OnSnapped(layerName, gridID, coord);

            return true;
        }

        private bool IsSpecialSnappableEntity(GameObject entity, out SnappableConfig config)
        {
            config = null;

            foreach (var special in specialSnappables)
            {
                if (entity == special.entity)
                {
                    config = special;
                    return true;
                }
            }

            return false;
        }
    }
}