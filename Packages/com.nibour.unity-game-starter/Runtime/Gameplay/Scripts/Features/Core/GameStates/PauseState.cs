using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;
using UnityGameStarter.PauseManagement;

namespace UnityGameStarter.Gameplay
{
    public class PauseState : BaseEventState<PauseState>
    {
        public PauseState(StateMachine stateMachine) : base(stateMachine) { }

        internal override void EnterState() 
        {
            if (PauseManager.TryGetInstance(out var instance))
                instance.SetPause(true);
            
            base.EnterState();
        }

        internal override void ExitState()
        {
            if (PauseManager.TryGetInstance(out var instance))
                instance.SetPause(false);

            base.ExitState();
        }
    }
}