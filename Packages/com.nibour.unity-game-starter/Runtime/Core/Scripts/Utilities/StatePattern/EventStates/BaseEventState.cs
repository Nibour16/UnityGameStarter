using UnityEngine;
using UnityGameStarter.Events;

namespace UnityGameStarter.FiniteStateMachine.EventState 
{
    public abstract class BaseEventState<T> : BaseState where T : BaseEventState<T>
    {
        public BaseEventState(StateMachine stateMachine) : base(stateMachine) { }

        internal override void EnterState()
        {
            EventManager.Instance.Publish(new EnterStateEvent<T>());
        }

        internal override void UpdateState()
        {
            EventManager.Instance.Publish(new UpdateStateEvent<T>());
        }

        internal override void ExitState()
        {
            EventManager.Instance.Publish(new ExitStateEvent<T>());
        }
    }
}
