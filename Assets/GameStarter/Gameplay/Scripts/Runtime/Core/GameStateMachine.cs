using System;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.Gameplay.Core
{
    public class GameStateMachine : BaseStateMachine
    {
        protected override bool SetDefault => false;
        
        protected override Type[] GetInitialStates()
        {
            return new[]
            {
                typeof(GameplayState),
                typeof(LoadingState),
                typeof(PauseState),
                typeof(ExitGameState)
            };
        }
    }
}