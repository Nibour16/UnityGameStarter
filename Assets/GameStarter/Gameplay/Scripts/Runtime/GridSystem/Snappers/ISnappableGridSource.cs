using System.Collections.Generic;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public interface ISnappableGridSource
    {
        IReadOnlyDictionary<Layer, Grid> Grids { get; }
    }
}