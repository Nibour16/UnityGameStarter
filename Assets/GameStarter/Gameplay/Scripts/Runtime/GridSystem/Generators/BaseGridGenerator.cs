using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Surface;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public abstract class BaseGridGenerator<T> : MonoBehaviour where T : class, ISurfaceSampler
    {
        [SerializeField] protected Vector3Int size;
        [SerializeField] protected float cellSize = 1f;
        [SerializeField] protected string[] layers;

        protected Dictionary<Layer, Grid> grids = new();
        private readonly Dictionary<string, Layer> _layersByName = new();
        private readonly Dictionary<int, Layer> _layersById = new();

        protected abstract Vector3Int Origin { get; }
        protected abstract T Sampler { get; }

        protected virtual void Awake() 
        {
            var bounds = new Bounds(transform.position, (Vector3)size * cellSize);
            GenerateGrids(bounds, Sampler);
        }

        private void GenerateGrids(Bounds bounds, ISurfaceSampler sampler)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                var grid = new Grid(bounds, Origin, cellSize, sampler);

                Layer layer = new()
                {
                    name = layers[i],
                    id = i
                };

                grids[layer] = grid;
                _layersByName[layer.name] = layer;
                _layersById[layer.id] = layer;
            }
        }

        protected Layer GetLayerByName(string name) => _layersByName[name];
        protected Layer GetLayerById(int id) => _layersById[id];

        public Grid GetGridByLayer(string layer) => grids[GetLayerByName(layer)];
        public Grid GetGridByLayer(int layer) => grids[GetLayerById(layer)];
    }
}