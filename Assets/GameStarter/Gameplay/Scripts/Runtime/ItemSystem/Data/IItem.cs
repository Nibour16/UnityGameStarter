using UnityEngine;

namespace UnityGameStarter.Gameplay.Item 
{
    public abstract class BaseItem : MonoBehaviour 
    {
        [SerializeField] protected ItemData data;

        public ItemData Data => data;
        internal virtual void OnAdded() { }
        internal virtual void OnRemoved() { }

        protected virtual void Awake() 
        {
            data.item = gameObject;
        }
    }
}