using UnityEngine;
using UnityGameStarter.EventSystem.EventManagement;

namespace UnityGameStarter.FiniteStateMachine.EventState 
{
    public abstract class BaseEventState<T> : BaseState where T : BaseEventState<T>
    {
        public BaseEventState(BaseStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            EventManager.Instance.Publish(new EnterStateEvent<T>());
        }

        public override void UpdateState()
        {
            EventManager.Instance.Publish(new UpdateStateEvent<T>());
        }

        public override void ExitState()
        {
            EventManager.Instance.Publish(new ExitStateEvent<T>());
        }
    }
}
