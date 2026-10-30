using UnityEngine;
using UnityGameStarter.SingletonPattern;

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

        private GridSnapper _gridSnapper;

        public GridSnapper Snapper => _gridSnapper;
        public IGridSource Source => source as IGridSource;

        private void Awake()
        {
            _gridSnapper = new GridSnapper(Source);
        }

        private void Start() => SnapAllEntities();

        /// <summary>
        /// Only entities with IGridSnappable interface component can be snapped during initialization
        /// </summary>
        private void SnapAllEntities()
        {
            if (!Source.IsGridGenerated) return;
            
            var entities = FindObjectsByType<GameObject>();
            string targetLayer = defaultLayer;
            bool setToBlock = defaultSetToBlockWhenSnapping;

            foreach(var entity in entities) 
            {
                if (!entity.TryGetComponent<IGridSnappable>(out _))
                    continue;
                
                if (IsSpecialSnappableEntity(entity, out var config)) 
                {
                    targetLayer = config.targetLayer;
                    setToBlock = config.setToBlock;
                }

                _gridSnapper.TrySnapToGrid(entity, targetLayer, out _, out _, setToBlock);
            }
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