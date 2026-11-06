using System;
using UnityEngine;
using UnityGameStarter.DataStructure;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.NewUpdateSystem
{
    /// <summary>
    /// An advanced manager for centralized update system.
    /// Usually supports for frameworks not gameplay logic.
    /// This allows non-monoBehaviour classes to have update methods.
    /// </summary>
    /// 
    /// <remarks>
    /// Appliers with the specific interface(s) must manually register themselves to make the update work
    /// </remarks>
    [RuntimeSingleton]
    public class UpdateManager : Singleton<UpdateManager>
    {
        // Three updatable collections with deferred list type
        private readonly DeferredList<IUpdatable> _updatables = new();
        private readonly DeferredList<IFixedUpdatable> _fixedUpdatables = new();
        private readonly DeferredList<ILateUpdatable> _lateUpdatables = new();

        #region API
        public void Register(object target)
        {
            if (target is IUpdatable update)
                Register(_updatables, update);

            if (target is IFixedUpdatable fixedUpdate)
                Register(_fixedUpdatables, fixedUpdate);

            if (target is ILateUpdatable lateUpdate)
                Register(_lateUpdatables, lateUpdate);
        }

        public void Unregister(object target)
        {
            if (target is IUpdatable update)
                Unregister(_updatables, update);

            if (target is IFixedUpdatable fixedUpdate)
                Unregister(_fixedUpdatables, fixedUpdate);

            if (target is ILateUpdatable lateUpdate)
                Unregister(_lateUpdatables, lateUpdate);
        }
        #endregion

        #region Registration Handlers
        private static void Register<T>(DeferredList<T> list, T item)
        {
            if (!list.Contains(item))
                list.EnqueueAdd(item);
        }

        private static void Unregister<T>(DeferredList<T> list, T item)
        {
            list.EnqueueRemove(item);
        }
        #endregion

        #region Life Cycle
        private void Update()
        {
            Process(_updatables, Time.deltaTime, (item, deltaTime) => item.CustomUpdate(deltaTime));
        }

        private void FixedUpdate()
        {
            Process(_fixedUpdatables, Time.fixedDeltaTime, (item, deltaTime) => item.CustomFixedUpdate(deltaTime));
        }

        private void LateUpdate()
        {
            Process(_lateUpdatables, Time.deltaTime, (item, deltaTime) => item.CustomLateUpdate(deltaTime));
        }

        /// <summary>
        /// Execute the update method of the each in the specific list. 
        /// Callback is defined as the item executes its update method with the delta time parameter as the input
        /// </summary>
        private static void Process<T>(DeferredList<T> list, float deltaTime, Action<T, float> callback)
        {
            list.ApplyRemovals();
            list.ApplyAdditions();

            for (int i = 0; i < list.Count; i++)
                callback(list[i], deltaTime);
        }
        #endregion
    }
}