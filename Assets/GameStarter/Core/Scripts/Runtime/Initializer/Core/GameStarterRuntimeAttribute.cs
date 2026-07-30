using System;
using UnityEngine;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.RuntimeCore 
{
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