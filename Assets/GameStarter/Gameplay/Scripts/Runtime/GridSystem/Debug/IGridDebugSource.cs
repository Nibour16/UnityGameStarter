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

        Vector3Int Origin(bool centerOrigin);
        Vector3 WorldOrigin(bool centerOrigin);

        Vector3 GridRight { get; }
        Vector3 GridUp { get; }
        Vector3 GridForward { get; }
    }
}