using UnityEngine;

namespace UnityGameStarter.Surface
{
    public interface ISurfaceSampler 
    {
        bool TrySample(Vector3 origin, Vector3 direction, float distance, out SurfaceSample sample);
    }

    public readonly struct SurfaceSample
    {
        public readonly Vector3 position;
        public readonly Vector3 normal;

        public SurfaceSample(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
        }
    }
}