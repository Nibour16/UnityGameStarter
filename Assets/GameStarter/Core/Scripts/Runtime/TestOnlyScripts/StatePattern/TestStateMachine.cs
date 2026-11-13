using System;
using UnityEngine;
using UnityGameStarter.FiniteStateMachine;

namespace UnityGameStarter.TestOnlyScripts.FiniteStateMachine
{
    public class TestStateMachine : StateMachine
    {
        protected override Type[] InitialStates => new[] { typeof(TestStateA), typeof(TestStateB) };

        protected override void Start()
        {
            Debug.Log("State Machine is ready");
            base.Start();
        }
    }
}