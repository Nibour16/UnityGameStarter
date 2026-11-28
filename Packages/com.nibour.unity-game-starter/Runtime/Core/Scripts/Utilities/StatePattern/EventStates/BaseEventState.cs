using UnityEngine;
using UnityGameStarter.Events;

namespace UnityGameStarter.FiniteStateMachine.EventState 
{
    public abstract class BaseEventState<T> : BaseState where T : BaseEventState<T>
    {
        public BaseEventState(StateMachine stateMachine) : base(stateMachine) { }

        protected override void OnEnter()
        {
            EventManager.Instance.Publish(new EnterStateEvent<T>());
        }

        protected override void OnUpdate()
        {
            EventManager.Instance.Publish(new UpdateStateEvent<T>());
        }

        protected override void OnExit()
        {
            EventManager.Instance.Publish(new ExitStateEvent<T>());
        }
    }
}
