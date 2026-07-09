using System;
using UnityEngine;
using UnityGameStarter.FiniteStateMachine;        

namespace UnityGameStarter.TestOnlyScripts 
{
    public class TestStateMachine : BaseStateMachine
    {
        protected override Type[] GetInitialStates() => new[] { typeof(TestStateA), typeof(TestStateB) };

        protected override void Start()
        {
            Debug.Log("State Machine is ready");
            base.Start();
        }
    }
}