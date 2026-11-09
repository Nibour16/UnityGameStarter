using System.Collections.Generic;
using UnityGameStarter.CommonData;

namespace UnityGameStarter.BuildCore 
{
    using UnityGameStarter.BootstrapLibrary;

    internal static class GameStarterBuildBootstrap<TContext> where TContext : GameStarterBuildContext
    {
        private static readonly List<Initializer<GameStarterBuildPhase, TContext>> _initializers = new();

        private static bool _discovered;

        public static void Execute(GameStarterBuildPhase phase, TContext context)
            => BootstrapLibrary.Execute(_initializers, phase, context);

        public static void Reset()
        {
            _initializers.Clear();
            _discovered = false;
        }

        public static void Discover()
        {
            if (_discovered) return;

            BootstrapLibrary.Discover<GameStarterBuildAttribute, GameStarterBuildPhase, TContext>(
                _initializers, x => x.Phase, x => (int)x);

            _discovered = true;
        }
    }
}