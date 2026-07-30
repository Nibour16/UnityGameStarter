using System;
using UnityGameStarter.StarterAttributes;

namespace UnityGameStarter.BuildCore 
{
    public enum GameStarterBuildPhase
    {
        BeforeBuild,
        Prepare,

        Prebake,
        Prevalidate,

        BuildScene,

        PostBake,
        PostValidate,

        AfterBuild
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GameStarterBuildAttribute : OrderedAttribute
    {
        public GameStarterBuildPhase Phase { get; }

        public GameStarterBuildAttribute(
            GameStarterBuildPhase phase,
            int order = 0) : base(order)
        {
            Phase = phase;
        }
    }
}