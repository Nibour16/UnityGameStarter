using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;

namespace UnityGameStarter.Gameplay
{
    public enum ExitGameReason
    {
        Exit,
        Restart,
        OpenNewLevel
    }

    public class EnterGameExitEvent : EnterStateEvent<ExitGameState>
    {
        public ExitGameReason Reason { get; }
        public Scene TargetScene { get; }

        public EnterGameExitEvent(ExitGameReason reason, Scene targetScene = default)
        {
            Reason = reason;
            TargetScene = targetScene;
        }
    }

    public class GameStateMachine : BaseStateMachine
    {
        protected override bool SetDefault => false;

        #region Game Exit
        [SerializeField] private bool alwaysUseDefaultExit = false;
        public bool AlwaysUseDefaultExit => alwaysUseDefaultExit;

        internal ExitGameReason reason = ExitGameReason.Exit;
        internal Scene nextTargetScene = default;
        #endregion

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