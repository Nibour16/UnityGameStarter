using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.FiniteStateMachine
{
    public class TestStateB : BaseState
    {
        public TestStateB(StateMachine stateMachine) : base(stateMachine){ }
        
        public override void EnterState()
        {
            Debug.Log("Enter State B");
        }

        public override void UpdateState()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState<TestStateA>();
        }

        public override void ExitState()
        {
            Debug.Log("Exit State B");
        }
    }
}