using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class PlaneGridGenerator : BaseGridGenerator<PlaneSurfaceSampler>
    {
        [SerializeField] private SpriteRenderer renderer;

        protected override PlaneSurfaceSampler Sampler => new(GridUp, GridCenter);

        public override Vector3Int CellCount
        {
            get
            {
                Vector3 worldSize = renderer.bounds.size;

                return new Vector3Int(
                    Mathf.CeilToInt(worldSize.x / CellSize),
                    Mathf.CeilToInt(worldSize.y / CellSize),
                    Mathf.CeilToInt(worldSize.z / CellSize));
            }
        }
    }
}