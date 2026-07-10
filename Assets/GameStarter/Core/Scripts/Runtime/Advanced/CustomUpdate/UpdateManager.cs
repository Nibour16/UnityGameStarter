using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap;

namespace UnityGameStarter.Advanced.CustomUpdate 
{
    public interface IUpdatable
    {
        void CustomUpdate(float deltaTime);
    }

    [RuntimeSingleton]
    public class UpdateManager : Singleton<UpdateManager>
    {
        private readonly List<IUpdatable> updatables = new();

        public void Register(IUpdatable updatable)
        {
            if (!updatables.Contains(updatable))
                updatables.Add(updatable);
        }

        public void Unregister(IUpdatable updatable)
        {
            updatables.Remove(updatable);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            
            // Optionally: for performance, iterate backwards if you allow removal during iteration
            foreach (var updatable in updatables)
            {
                updatable.CustomUpdate(deltaTime);
            }
        }
    }
}