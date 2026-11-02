using UnityEngine;

namespace UnityGameStarter.Gameplay.Item 
{
    public abstract class BaseItem : MonoBehaviour 
    {
        [SerializeField] protected ItemData data;

        public ItemData Data => data;
        public virtual void OnAdded() { }
        public virtual void OnRemoved() { }

        protected virtual void Awake() 
        {
            data.item = gameObject;
        }
    }
}