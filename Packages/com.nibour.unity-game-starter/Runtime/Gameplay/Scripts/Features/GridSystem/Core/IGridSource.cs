using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public interface IGridSource
    {
        bool IsGridGenerated { get; }
        float CellSize { get; }
        IReadOnlyDictionary<Layer, Grid> Grids { get; }

        bool TryGetLayerByName(string name, out Layer layer);
        bool TryGetGridByLayerName(string name, out Grid grid);

        Vector3Int GetCellCount(Layer layer);
        Vector3 GetGridRight(Layer layer);
        Vector3 GetGridUp(Layer layer);
        Vector3 GetGridForward(Layer layer);
    }
}