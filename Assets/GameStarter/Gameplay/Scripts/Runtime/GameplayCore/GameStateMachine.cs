using System;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.Gameplay.Core
{
    public class GameStateMachine : BaseStateMachine
    {
        protected override Type[] GetInitialStates()
        {
            return new[]
            {
                typeof(LoadingState),
                typeof(GameplayState),
                typeof(PauseState),
                typeof(ExitGameState)
            };
        }
    }
}