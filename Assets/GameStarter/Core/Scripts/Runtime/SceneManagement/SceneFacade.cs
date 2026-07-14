using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.ServiceLocatorPattern;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.SingletonPattern.RuntimeSingletonBootstrap;

namespace UnityGameStarter.SceneManagement
{
    [RuntimeSingleton]
    public class SceneFacade : Singleton<SceneFacade>, IService
    {
        public event Action<string> SceneLoadStarted;
        public event Action<string> SceneLoaded;

        public string ActiveSceneName
            => SceneManager.GetActiveScene().name;

        public Scene ActiveScene
            => SceneManager.GetActiveScene();

        #region Life Cycle
        private void OnEnable()
        {
            ServiceLocator.Register(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ServiceLocator.Unregister<SceneFacade>();
        }
        #endregion

        #region Load
        public void Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneLoadStarted?.Invoke(sceneName);

            SceneManager.LoadScene(sceneName, mode);
        }

        public async Task LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneLoadStarted?.Invoke(sceneName);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);

            if (operation == null)
            {
                Debug.LogError($"Scene {sceneName} cannot be loaded.");
                return;
            }

            while (!operation.isDone)
                await Task.Yield();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneLoaded?.Invoke(scene.name);
        }
        #endregion

        #region Unload
        public async Task UnloadAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

            if (operation == null)
                return;

            while (!operation.isDone)
                await Task.Yield();
        }
        #endregion

        #region Reload

        public Task ReloadAsync()
        {
            return LoadAsync(ActiveSceneName);
        }
        #endregion
    }
}