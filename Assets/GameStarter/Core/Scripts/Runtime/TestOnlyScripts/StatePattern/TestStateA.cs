using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.StateMachine
{
    public class TestStateA : BaseState
    {
        public TestStateA(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            Debug.Log("Enter State A");
        }

        public override void UpdateState()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState(typeof(TestStateB));
        }

        public override void ExitState()
        {
            Debug.Log("Exit State A");
        }
    }
}