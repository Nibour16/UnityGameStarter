using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;
using UnityGameStarter.PauseManagement;

namespace UnityGameStarter.Gameplay
{
    public class PauseState : BaseEventState<PauseState>
    {
        public PauseState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState() 
        {
            if (PauseManager.TryGetInstance(out var instance))
                instance.SetPause(true);
            
            base.EnterState();
        }

        public override void ExitState()
        {
            if (PauseManager.TryGetInstance(out var instance))
                instance.SetPause(false);

            base.ExitState();
        }
    }
}