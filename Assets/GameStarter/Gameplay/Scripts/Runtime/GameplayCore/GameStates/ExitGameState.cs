using UnityEngine;
using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.Gameplay.LevelManagement;

namespace UnityGameStarter.Gameplay.Core 
{
    public class ExitGameState : BaseState
    {
        public ExitGameState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            LevelManager.Instance.ExitLevel();
        }

        public override void UpdateState() { }

        public override void ExitState() { }
    }
}