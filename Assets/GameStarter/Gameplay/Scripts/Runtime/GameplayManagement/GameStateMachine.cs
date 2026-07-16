using System;
using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.Gameplay.Core
{
    public class GameStateMachine : BaseStateMachine
    {
        internal EventManager EventManager { get; private set; }
        
        protected override void Awake() 
        {
            base.Awake();
            EventManager = EventManager.Instance;
        }

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