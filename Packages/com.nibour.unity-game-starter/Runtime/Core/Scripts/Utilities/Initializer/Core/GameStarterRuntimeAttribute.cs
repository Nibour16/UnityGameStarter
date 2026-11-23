using System;
using UnityEngine;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.RuntimeCore 
{
    /// <summary>
    /// A method attribute that connects the target method to specific runtime initialization load type.
    /// Unity also gives the attribute to apply those, but this one supports the customized order
    /// </summary>
    /// <remarks>
    /// Even though the order does not matter, still recommended to use this one for uniformity
    /// (which honestly it does not matter about the logic usually)
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GameStarterRuntimeAttribute : OrderedAttribute
    {
        public RuntimeInitializeLoadType LoadType { get; }

        public GameStarterRuntimeAttribute(
            RuntimeInitializeLoadType loadType, int order = 0) : base(order)
        {
            LoadType = loadType;
        }
    }
}