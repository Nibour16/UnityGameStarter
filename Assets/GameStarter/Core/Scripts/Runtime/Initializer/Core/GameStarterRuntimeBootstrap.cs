using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.CommonData;

namespace UnityGameStarter.RuntimeCore 
{
    using UnityGameStarter.BootstrapLibrary;

    static class GameStarterRuntimeBootstrap
    {
        private static readonly List<Initializer<RuntimeInitializeLoadType>> _initializers = new();
        private static bool _discovered;

        private static int GetLoadOrder(RuntimeInitializeLoadType type)
        {
            return type switch
            {
                RuntimeInitializeLoadType.BeforeSplashScreen => 0,
                RuntimeInitializeLoadType.SubsystemRegistration => 1,
                RuntimeInitializeLoadType.AfterAssembliesLoaded => 2,
                RuntimeInitializeLoadType.BeforeSceneLoad => 3,
                RuntimeInitializeLoadType.AfterSceneLoad => 4,
                _ => int.MaxValue
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void BeforeSplashScreen()
        {
            Execute(RuntimeInitializeLoadType.BeforeSplashScreen);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistration()
        {
            Reset();
            Discover();

            Execute(RuntimeInitializeLoadType.SubsystemRegistration);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void AfterAssembliesLoaded()
        {
            Execute(RuntimeInitializeLoadType.AfterAssembliesLoaded);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            Execute(RuntimeInitializeLoadType.BeforeSceneLoad);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            Execute(RuntimeInitializeLoadType.AfterSceneLoad);
        }

        private static void Execute(RuntimeInitializeLoadType loadType)
        {
            BootstrapLibrary.Execute(_initializers, loadType);
        }

        private static void Reset() 
        {
            _initializers.Clear();
            _discovered = false;
        }

        private static void Discover()
        {
            if (_discovered) return;

            BootstrapLibrary.Discover<GameStarterRuntimeAttribute, RuntimeInitializeLoadType>(
                _initializers, x => x.LoadType, GetLoadOrder);

            _discovered = true;
        }
    }
}