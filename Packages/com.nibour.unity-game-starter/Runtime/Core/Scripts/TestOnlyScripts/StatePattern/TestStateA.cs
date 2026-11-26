using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.FiniteStateMachine
{
    public class TestStateA : BaseState
    {
        public TestStateA(StateMachine stateMachine) : base(stateMachine) { }

        internal override void EnterState()
        {
            Debug.Log("Enter State A");
        }

        internal override void UpdateState()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState<TestStateB>();
        }

        internal override void ExitState()
        {
            Debug.Log("Exit State A");
        }
    }
}