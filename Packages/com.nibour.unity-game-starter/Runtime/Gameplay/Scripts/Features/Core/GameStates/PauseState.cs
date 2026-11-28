using UnityGameStarter.FiniteStateMachine;
using UnityGameStarter.FiniteStateMachine.EventState;
using UnityGameStarter.PauseManagement;

namespace UnityGameStarter.Gameplay
{
    public class PauseState : BaseEventState<PauseState>
    {
        public PauseState(StateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter() 
        {
            if (PauseManager.TryGetInstance(out var instance))
                instance.SetPause(true);
            
            base.OnEnter();
        }

        protected override void OnExit()
        {
            if (PauseManager.TryGetInstance(out var instance))
                instance.SetPause(false);

            base.OnExit();
        }
    }
}