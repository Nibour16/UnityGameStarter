using UnityEngine;

namespace UnityGameStarter.SingletonPattern 
{
    public abstract class OptionalSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        [SerializeField] private bool isStatic = true;

        protected override MonoBehaviour InstanceRef => isStatic ? base.InstanceRef : null;
    }
}