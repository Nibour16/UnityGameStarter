using UnityEngine;

namespace UnityGameStarter.SingletonPattern 
{
    /// <summary>
    /// Variant base class of singleton for MonoBehaviours to make the singleton instance as optional.
    /// For example if IsStatic boolean property is true, it is a singleton, 
    /// if false then it will be acted as a normal MonoBehaviour
    /// </summary>
    public abstract class OptionalSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected abstract bool IsStatic { get; }
        protected override MonoBehaviour InstanceRef => IsStatic ? base.InstanceRef : null;
    }
}