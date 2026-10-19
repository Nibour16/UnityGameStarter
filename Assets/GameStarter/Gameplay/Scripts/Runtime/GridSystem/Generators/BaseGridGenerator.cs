using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    [System.Serializable]
    public class LayerConfig
    {
        public string layerName = "Default";
        public GridState initialState = GridState.Empty;
    }

    public abstract class BaseGridGenerator<TSurface, TLayer> : MonoBehaviour, IGridDebugSource
        where TSurface : class, ISurfaceSampler where TLayer : LayerConfig
    {
        #region Configuration
        [SerializeField] protected TLayer[] layers = new TLayer[1];
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
        #endregion

        #region Grid Properties
        public float CellSize => cellSize;
        public Vector3 GridSize => (Vector3)GetCellCount(grids.Keys.FirstOrDefault()) * cellSize;

        public GridOriginType OriginType => originType;
        public CenterOriginMode CenterOriginMode => centerOriginMode;

        protected Vector3 MainGridCenter => GetGridCenter(grids.Keys.FirstOrDefault());
        protected Vector3 MainGridUp => GetGridUp(grids.Keys.FirstOrDefault());
        protected Vector3 MainGridRight => GetGridRight(grids.Keys.FirstOrDefault());
        protected Vector3 MainGridForward => GetGridForward(grids.Keys.FirstOrDefault());
        protected Vector3Int MainCellCount => GetCellCount(grids.Keys.FirstOrDefault());
        #endregion

        #region Generator Configuration
        protected abstract Vector3 SampleDirection { get; }

        protected virtual Vector3 CellFaceDirection => SampleDirection;
        protected virtual bool IncludeIncompleteCell => false;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            Initialize();
        }
        #endregion

        #region Layer Access
        public Layer GetLayerByName(string name) => _layersByName[name];
        public Layer GetLayerById(int id) => _layersById[id];

        public Grid GetGridByLayer(string layer) => grids[GetLayerByName(layer)];
        public Grid GetGridByLayer(int layer) => grids[GetLayerById(layer)];

        protected TLayer GetSerializedLayer(Layer layer) 
            => System.Array.Find(layers, t => t.layerName == layer.name);
        #endregion

        #region Grid Generation
        private void Initialize() 
        {
            grids.Clear();
            _layersByName.Clear();
            _layersById.Clear();
            _isGridGenerated = false;

            CollectLayers();
            var bounds = new Bounds(MainGridCenter, GridSize);
            GenerateGrids(bounds);
        }

        private void CollectLayers() 
        {
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

                Layer layer = new() { name = layerName, id = i };

                grids.Add(layer, null);
                _layersByName.Add(layer.name, layer);
                _layersById.Add(layer.id, layer);
            }
        }

        private void GenerateGrids(Bounds bounds)
        {
            foreach (var config in layers ?? Array.Empty<TLayer>())
            {
                if (string.IsNullOrWhiteSpace(config.layerName)) continue;

                if (!_layersByName.TryGetValue(config.layerName, out Layer layer)) continue;

                Grid grid = new(bounds, MainGridCenter, cellSize, Sampler(layer), SampleDirection,
                    CellFaceDirection, config.initialState, IncludeIncompleteCell, 
                    originType, centerOriginMode);

                grids[layer] = grid;
            }

            _isGridGenerated = true;
        }
        #endregion

        #region Grid Properties of Specific Layer
        protected virtual Vector3 GetGridCenter(Layer layer) => transform.position;
        public virtual Vector3 GetGridRight(Layer layer) => transform.right;
        public virtual Vector3 GetGridUp(Layer layer) => transform.up;
        public virtual Vector3 GetGridForward(Layer layer) => transform.forward;

        public abstract Vector3Int GetCellCount(Layer layer);
        #endregion

        #region Sampler Configeration
        protected abstract TSurface Sampler(Layer layer);
        #endregion
    }
}