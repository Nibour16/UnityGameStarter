using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class PlaneGridGenerator : BaseGridGenerator<PlaneSurfaceSampler>
    {
        protected override PlaneSurfaceSampler Sampler => new(transform.up, transform.position);

        protected override Vector3Int Origin => throw new System.NotImplementedException();
    }
}