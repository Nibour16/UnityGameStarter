using System;
using UnityEngine;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.RuntimeCore 
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GameStarterAttribute : OrderedAttribute
    {
        public RuntimeInitializeLoadType LoadType { get; }

        public GameStarterAttribute(RuntimeInitializeLoadType loadType, int order = 0) : base(order)
        {
            LoadType = loadType;
        }
    }
}