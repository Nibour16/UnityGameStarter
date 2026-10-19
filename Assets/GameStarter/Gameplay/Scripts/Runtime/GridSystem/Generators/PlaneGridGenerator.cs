using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class PlaneGridGenerator : BaseGridGenerator<PlaneSurfaceSampler, LayerConfig>
    {
        [SerializeField] private SpriteRenderer renderer;

        protected override PlaneSurfaceSampler Sampler(Layer layer) => new(MainGridUp, MainGridCenter);
        protected override Vector3 SampleDirection => Vector3.up;

        public override Vector3Int GetCellCount(Layer layer)
        {
            Vector3 worldSize = renderer.bounds.size;

            return new Vector3Int(Mathf.CeilToInt(worldSize.x / CellSize),
                Mathf.CeilToInt(worldSize.y / CellSize),
                Mathf.CeilToInt(worldSize.z / CellSize));
        }
    }
}