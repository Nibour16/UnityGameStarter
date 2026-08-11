using System;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public enum ExitGameReason
    {
        Exit,
        Restart
    }

    public class EnterGameExitEvent : EnterStateEvent<ExitGameState>
    {
        public ExitGameReason Reason { get; }

        public EnterGameExitEvent(ExitGameReason reason)
        {
            Reason = reason;
        }
    }

    public class GameStateMachine : BaseStateMachine
    {
        protected override bool SetDefault => false;
        public ExitGameReason reason = ExitGameReason.Exit;

        protected override Type[] GetInitialStates()
        {
            return new[]
            {
                typeof(InitialGameState),
                typeof(LoadingState),
                typeof(GameplayState),
                typeof(PauseState),
                typeof(ExitGameState)
            };
        }
    }
}