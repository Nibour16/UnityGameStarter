using UnityEngine;

namespace UnityGameStarter.Gameplay.GridSystem.TestOnly
{
    public sealed class TestGridEntity : MonoBehaviour, IGridCollideEvent, IGridSnappable
    {
        public void OnHit(GameObject other)
        {
            if (other.CompareTag("Player"))
                Debug.Log("Hey, it's you, player!");
        }

        public void OnSnapped(string snappedLayer, Vector3Int gridID, Vector3Int coord)
        {
            Debug.Log(
                $"Me as the entity '{gameObject.name}' has snapped into the layer '{snappedLayer}'.\n" +
                $"My world position is: {transform.position}, and my grid coordinate is: {coord}");
        }
    }
}