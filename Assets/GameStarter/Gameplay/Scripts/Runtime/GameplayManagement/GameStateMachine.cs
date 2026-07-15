using System;
using UnityEngine;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.Gameplay.Core
{
    public class GameStateMachine : BaseStateMachine
    {
        protected override Type[] GetInitialStates()
        {
            return new[]
            {
                typeof(InitialGameState),
                typeof(LoadingState),
                typeof(GameplayState),
                typeof(PauseState)
            };
        }
    }
}