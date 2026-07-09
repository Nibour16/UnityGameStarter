using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts 
{
    public class TestStateB : BaseState
    {
        public TestStateB(BaseStateMachine stateMachine) : base(stateMachine){ }
        
        public override void EnterState()
        {
            Debug.Log("Enter State B");
        }

        public override void UpdateState()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                stateMachine.SetState(typeof(TestStateA));
        }

        public override void ExitState()
        {
            Debug.Log("Exit State B");
        }
    }
}