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
        public GridState initialState = GridState.Passable;
    }

    public abstract class BaseGridGenerator<TSurface, TLayer> : MonoBehaviour, IGridSource
        where TSurface : class, ISurfaceSampler where TLayer : LayerConfig
    {
        #region Configuration
        [SerializeField] protected TLayer[] layers = new TLayer[1];
        [SerializeField] private float cellSize = 1f;
        
        [SerializeField] private GridOriginType originType = GridOriginType.Center;

        [Header("Advanced")]
        [SerializeField] private CenterOriginMode centerOriginMode = CenterOriginMode.CellCenter;
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
        public bool TryGetLayerByName(string name, out Layer layer) 
        {
            layer = default;

            if (!_layersByName.Keys.Contains(name)) return false;
            
            layer = _layersByName[name];
            return true;
        }
        public bool TryGetLayerByID(int id, out Layer layer) 
        {
            layer = default;

            if (!_layersById.Keys.Contains(id)) return false;

            layer = _layersById[id];
            return true;
        }

        public bool TryGetGridByLayerName(string name, out Grid grid) 
        {
            grid = null;

            if (!TryGetLayerByName(name, out var layer)) return false;

            grid = grids[layer];
            return true;
        }

        public bool TryGetGridByLayerID(int id, out Grid grid) 
        {
            grid = null;

            if (!TryGetLayerByID(id, out var layer)) return false;

            grid = grids[layer];
            return true;
        }

        protected TLayer GetLayerConfig(Layer layer) 
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

            if (grids.Count == 0)
            {
                Debug.LogError("No valid grid layers were collected.", this);
                return;
            }

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
            foreach (var config in layers ?? System.Array.Empty<TLayer>())
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