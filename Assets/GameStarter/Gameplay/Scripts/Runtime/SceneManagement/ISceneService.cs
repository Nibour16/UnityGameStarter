using System;
using UnityEngine.SceneManagement;
using UnityGameStarter.ServiceLocatorPattern;

namespace UnityGameStarter.SceneManagement 
{
    public interface ISceneService : IService
    {
        void BindSceneLoadStart(Action<Scene, LoadSceneMode> e);
        void UnbindSceneLoadStart(Action<Scene, LoadSceneMode> e);

        void BindSceneLoaded(Action<Scene, LoadSceneMode> e);
        void UnbindSceneLoaded(Action<Scene, LoadSceneMode> e);

        void BindSceneUnloaded(Action<Scene> e);
        void UnbindSceneUnloaded(Action<Scene> e);
    }
}