using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.Gameplay.PauseManagement;

namespace UnityGameStarter.Gameplay.Core
{
    public class PauseState : BaseState
    {
        public PauseState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            PauseManager.Instance.SetPause(true);
        }

        public override void UpdateState() { }

        public override void ExitState()
        {
            PauseManager.Instance.SetPause(false);
        }
    }
}