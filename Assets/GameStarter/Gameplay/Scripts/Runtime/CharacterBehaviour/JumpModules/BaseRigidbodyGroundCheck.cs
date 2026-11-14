using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule 
{
    public abstract class BaseRigidbodyGroundCheck<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected float maxCheckDistance = 5f;

        protected T collider;

        protected virtual void Awake() 
        {
            collider = GetComponent<T>();

            if (!(collider is Collider || collider is Collider2D))
            {
                Debug.LogError("The generic type 'T' must be either type of collider or collider2D");
                return;
            }

            if (collider == null)
                Debug.LogError("Collider component is not detected in the current gameObject, " +
                    "not the child or parent");
        }
    }
}