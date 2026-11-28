using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class SpriteGridGenerator : BaseGridGenerator<PlaneSurfaceSampler, LayerConfig>
    {
        [SerializeField] private SpriteRenderer renderer;

        protected override PlaneSurfaceSampler Sampler(Layer layer) => new(MainGridCenter, MainGridForward);
        protected override Vector3 SampleDirection => -MainGridForward;

        public override Vector3Int GetCellCount(Layer layer)
        {
            Vector3 worldSize = renderer.bounds.size;

            return new Vector3Int(Mathf.CeilToInt(worldSize.x / CellSize),
                Mathf.CeilToInt(worldSize.y / CellSize),
                Mathf.CeilToInt(worldSize.z / CellSize));
        }
    }
}