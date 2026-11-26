using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.FiniteStateMachine
{
    public class TestStateB : BaseState
    {
        public TestStateB(StateMachine stateMachine) : base(stateMachine){ }
        
        internal override void EnterState()
        {
            Debug.Log("Enter State B");
        }

        internal override void UpdateState()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState<TestStateA>();
        }

        internal override void ExitState()
        {
            Debug.Log("Exit State B");
        }
    }
}