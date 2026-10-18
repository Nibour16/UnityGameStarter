using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public interface IGridDebugSource
    {
        bool IsGridGenerated { get; }
        IReadOnlyDictionary<Layer, Grid> Grids { get; }

        Vector3Int CellCount { get; }
        float CellSize { get; }

        Vector3 GridRight { get; }
        Vector3 GridUp { get; }
        Vector3 GridForward { get; }

        GridOriginType OriginType { get; }
        CenterOriginMode CenterOriginMode { get; }

        Grid GetGridByLayer(string layer);
    }
}