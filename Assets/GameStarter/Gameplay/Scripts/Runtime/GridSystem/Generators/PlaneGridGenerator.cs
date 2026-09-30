using System.Drawing;
using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public class PlaneGridGenerator : BaseGridGenerator<PlaneSurfaceSampler>
    {
        [SerializeField] private bool centerAsOrigin = false;
        
        protected override PlaneSurfaceSampler Sampler => new(transform.up, transform.position);

        public override Vector3Int Origin 
        {
            get
            {
                if (centerAsOrigin)
                    return new Vector3Int(-size.x / 2, -size.y / 2, -size.z / 2);
                else
                    return Vector3Int.zero;
            }
        }

        public override Vector3 WorldOrigin
        {
            get
            {
                if (centerAsOrigin)
                    return transform.position;
                else
                {
                    Vector3 halfSize = (Vector3)size * cellSize * 0.5f;
                    return transform.position - transform.right * halfSize.x - transform.forward * halfSize.z;
                }
            }
        }
    }
}