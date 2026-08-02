using System;
using UnityEngine.SceneManagement;
using UnityGameStarter.ServiceLocatorPattern;

namespace UnityGameStarter.SceneManagement 
{
    public interface ISceneService : IService
    {
        void BindSceneLoadStart(Action<Scene> e);
        void UnbindSceneLoadStart(Action<Scene> e);

        void BindSceneLoaded(Action<Scene> e);
        void UnbindSceneLoaded(Action<Scene> e);

        void BindSceneUnloaded(Action<Scene> e);
        void UnbindSceneUnloaded(Action<Scene> e);
    }
}