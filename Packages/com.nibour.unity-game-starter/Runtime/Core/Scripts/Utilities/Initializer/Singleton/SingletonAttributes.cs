using System;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.SingletonPattern 
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RuntimeSingletonAttribute : OrderedAttribute
    {
        public RuntimeSingletonAttribute(int order = 0) : base(order) { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class SingletonResetAttribute : Attribute { }
}
