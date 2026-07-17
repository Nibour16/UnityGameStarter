using UnityEngine;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.Gameplay.LevelManagement;

namespace UnityGameStarter.Gameplay.Core
{
    public class LoadingState : BaseState
    {
        public LoadingState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            EventManager.Instance.Register(this);
            LevelManager.Instance.InitializeLevel();
        }

        public override void UpdateState() { }

        public override void ExitState()
        {
            EventManager.Instance.Unregister(this);
        }

        [EventListener]
        private void InitializeComplete(InitializeLevelCompleteEvent e)
        {
            stateMachine.SetState(typeof(GameplayState));
        }
    }
}