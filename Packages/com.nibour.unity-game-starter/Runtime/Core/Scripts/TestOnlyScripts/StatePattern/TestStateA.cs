using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.FiniteStateMachine
{
    public class TestStateA : BaseState
    {
        public TestStateA(StateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            Debug.Log("Enter State A");
        }

        protected override void OnUpdate()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState<TestStateB>();
        }

        protected override void OnExit()
        {
            Debug.Log("Exit State A");
        }
    }
}