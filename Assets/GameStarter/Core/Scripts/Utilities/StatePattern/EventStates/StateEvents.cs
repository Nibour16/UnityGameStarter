using UnityEngine;

namespace UnityGameStarter.FiniteStateMachine.EventState 
{
    public abstract class BaseStateEvent<T> where T : BaseState { }

    public class EnterStateEvent<T> : BaseStateEvent<T> where T : BaseState { }

    public class UpdateStateEvent<T> : BaseStateEvent<T> where T : BaseState { }

    public class ExitStateEvent<T> : BaseStateEvent<T> where T : BaseState { }
}