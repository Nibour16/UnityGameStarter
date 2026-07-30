using System;
using UnityEngine;
using UnityGameStarter.DefferedDataStructure;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.NewUpdateSystem.Advanced
{
    [RuntimeSingleton]
    public class UpdateManager : Singleton<UpdateManager>
    {
        private readonly DefferedList<IUpdatable> _updatables = new();
        private readonly DefferedList<IFixedUpdatable> _fixedUpdatables = new();
        private readonly DefferedList<ILateUpdatable> _lateUpdatables = new();

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
        private static void Register<T>(DefferedList<T> list, T item)
        {
            if (!list.Contains(item))
                list.EnqueueAdd(item);
        }

        private static void Unregister<T>(DefferedList<T> list, T item)
        {
            list.EnqueueRemove(item);
        }
        #endregion

        #region Life Cycle
        private void Update()
        {
            Process(_updatables, Time.deltaTime, static (item, dt) => item.CustomUpdate(dt));
        }

        private void FixedUpdate()
        {
            Process(_fixedUpdatables, Time.fixedDeltaTime, static (item, dt) => item.CustomFixedUpdate(dt));
        }

        private void LateUpdate()
        {
            Process(_lateUpdatables, Time.deltaTime, static (item, dt) => item.CustomLateUpdate(dt));
        }

        private static void Process<T>(DefferedList<T> list, float deltaTime, Action<T, float> callback)
        {
            list.ApplyRemovals();
            list.ApplyAdditions();

            for (int i = 0; i < list.Count; i++)
            {
                callback(list[i], deltaTime);
            }
        }
        #endregion
    }
}