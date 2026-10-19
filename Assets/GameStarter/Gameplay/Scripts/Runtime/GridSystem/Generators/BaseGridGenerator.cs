using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    [System.Serializable]
    public class SerializedLayer
    {
        public string layerName;
        public GridState initialState = GridState.Empty;
    }

    public abstract class BaseGridGenerator<TSurface, TLayer> : MonoBehaviour, IGridDebugSource
        where TSurface : class, ISurfaceSampler where TLayer : SerializedLayer
    {
        #region Configuration
        [SerializeField] protected TLayer[] layers;
        [SerializeField] private float cellSize = 1f;
        
        [SerializeField] private GridOriginType originType = GridOriginType.Center;
        [SerializeField] private CenterOriginMode centerOriginMode = CenterOriginMode.Intersection;
        #endregion

        #region Grid Data
        private readonly Dictionary<string, Layer> _layersByName = new();
        private readonly Dictionary<int, Layer> _layersById = new();

        private bool _isGridGenerated;

        protected Dictionary<Layer, Grid> grids = new();
        public bool IsGridGenerated => _isGridGenerated;
        public IReadOnlyDictionary<Layer, Grid> Grids => grids;
        #endregion

        #region Grid Properties
        public float CellSize => cellSize;
        public Vector3 GridSize => (Vector3)CellCount * cellSize;

        public GridOriginType OriginType => originType;
        public CenterOriginMode CenterOriginMode => centerOriginMode;

        protected virtual Vector3 GridCenter => transform.position;

        public virtual Vector3 GridRight => transform.right;
        public virtual Vector3 GridUp => transform.up;
        public virtual Vector3 GridForward => transform.forward;

        public abstract Vector3Int CellCount { get; }
        #endregion

        #region Generator Configuration
        protected abstract TSurface Sampler { get; }
        protected abstract Vector3 SampleDirection { get; }

        protected virtual Vector3 CellFaceDirection => SampleDirection;
        protected virtual bool IncludeIncompleteCell => false;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            var bounds = new Bounds(GridCenter, GridSize);
            GenerateGrids(bounds, Sampler);
        }
        #endregion

        #region Layer Access
        protected Layer GetLayerByName(string name) => _layersByName[name];
        protected Layer GetLayerById(int id) => _layersById[id];

        public Grid GetGridByLayer(string layer) => grids[GetLayerByName(layer)];
        public Grid GetGridByLayer(int layer) => grids[GetLayerById(layer)];
        #endregion

        #region Grid Generation
        private void GenerateGrids(Bounds bounds, ISurfaceSampler sampler)
        {
            grids.Clear();
            _layersByName.Clear();
            _layersById.Clear();

            _isGridGenerated = false;

            if (layers == null) return;

            for (int i = 0; i < layers.Length; i++)
            {
                string layerName = layers[i].layerName;

                if (string.IsNullOrWhiteSpace(layerName))
                {
                    Debug.LogWarning($"Layer name at index {i} is empty.", this);
                    continue;
                }

                if (_layersByName.ContainsKey(layerName))
                {
                    Debug.LogWarning($"Duplicate layer name: {layerName}", this);
                    continue;
                }

                Layer layer = new(){ name = layerName, id = i };

                Grid grid = new(bounds, GridCenter, cellSize,
                    sampler, SampleDirection, CellFaceDirection, 
                    layers[i].initialState, IncludeIncompleteCell, originType, centerOriginMode);

                grids.Add(layer, grid);
                _layersByName.Add(layer.name, layer);
                _layersById.Add(layer.id, layer);
            }

            _isGridGenerated = true;
        }
        #endregion
    }
}