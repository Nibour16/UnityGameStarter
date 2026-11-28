using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.FiniteStateMachine
{
    public class TestStateB : BaseState
    {
        public TestStateB(StateMachine stateMachine) : base(stateMachine){ }

        protected override void OnEnter()
        {
            Debug.Log("Enter State B");
        }

        protected override void OnUpdate()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState<TestStateA>();
        }

        protected override void OnExit()
        {
            Debug.Log("Exit State B");
        }
    }
}