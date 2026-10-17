using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public abstract class BaseGridGenerator<T> : MonoBehaviour, IGridDebugSource
        where T : class, ISurfaceSampler
    {
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private string[] layers;

        private readonly Dictionary<string, Layer> _layersByName = new();
        private readonly Dictionary<int, Layer> _layersById = new();
        private bool _isGridGenerated = false;

        protected Dictionary<Layer, Grid> grids = new();
        protected string[] Layers => layers;

        public float CellSize => cellSize;
        public Vector3 GridSize => (Vector3)CellCount * cellSize;

        protected virtual Vector3 GridCenter => transform.position;
        public virtual Vector3 GridRight => transform.right;
        public virtual Vector3 GridUp => transform.up;
        public virtual Vector3 GridForward => transform.forward;

        public abstract Vector3Int CellCount { get; }
        protected abstract T Sampler { get; }

        public bool IsGridGenerated => _isGridGenerated;
        public IReadOnlyDictionary<Layer, Grid> Grids => grids;

        protected virtual void Awake() 
        {
            var bounds = new Bounds(GridCenter, GridSize);
            GenerateGrids(bounds, Sampler);
        }

        public Vector3Int Origin(bool centerOrigin)
        {
            if (centerOrigin)
                return new Vector3Int(-CellCount.x / 2, -CellCount.y / 2, -CellCount.z / 2);
            else
                return Vector3Int.zero;
        }

        public Vector3 WorldOrigin(bool centerOrigin)
        {
            if (centerOrigin)
                return GridCenter;

            Vector3 halfSize = 0.5f * GridSize;

            return GridCenter - transform.right * halfSize.x
                - transform.up * halfSize.y
                - transform.forward * halfSize.z;
        }

        private void GenerateGrids(Bounds bounds, ISurfaceSampler sampler)
        {
            grids.Clear();
            _layersByName.Clear();
            _layersById.Clear();

            if (layers == null) return;

            for (int i = 0; i < layers.Length; i++)
            {
                string layerName = layers[i];

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

                Layer layer = new()
                {
                    name = layerName,
                    id = i
                };

                Grid grid = new(bounds, Origin(true), WorldOrigin(true), cellSize, sampler);

                grids.Add(layer, grid);
                _layersByName.Add(layer.name, layer);
                _layersById.Add(layer.id, layer);
            }

            _isGridGenerated = true;
        }

        protected Layer GetLayerByName(string name) => _layersByName[name];
        protected Layer GetLayerById(int id) => _layersById[id];

        public Grid GetGridByLayer(string layer) => grids[GetLayerByName(layer)];
        public Grid GetGridByLayer(int layer) => grids[GetLayerById(layer)];
    }
}