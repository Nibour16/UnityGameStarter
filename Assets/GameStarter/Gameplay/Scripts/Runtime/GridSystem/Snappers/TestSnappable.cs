using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem 
{
    public sealed class TestSnappable : MonoBehaviour, IGridSnappable
    {
        public void OnSnapped(string snappedLayer, Vector3Int coord)
        {
            Debug.Log(
                $"Me as the entity '{gameObject.name}' has snapped into the layer '{snappedLayer}'.\n" +
                $"My world position is: {transform.position}, and my grid coordinate is: {coord}");
        }
    }
}