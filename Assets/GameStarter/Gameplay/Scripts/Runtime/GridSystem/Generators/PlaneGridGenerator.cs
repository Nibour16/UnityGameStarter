using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class PlaneGridGenerator : BaseGridGenerator<PlaneSurfaceSampler>
    {
        [SerializeField] private bool centerAsOrigin = false;
        
        protected override PlaneSurfaceSampler Sampler => new(transform.up, transform.position);

        protected override Vector3Int Origin 
        {
            get
            {
                if (centerAsOrigin)
                    return new Vector3Int(-size.x / 2, -size.y / 2, -size.z / 2);
                else
                    return Vector3Int.zero;
            }
        }
    }
}