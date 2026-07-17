using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.ServiceLocatorPattern;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.SceneManagement
{
    [RuntimeSingleton]
    public class SceneFacade : Singleton<SceneFacade>, ISceneService
    {
        private event Action<Scene> SceneLoadStarted;
        private event Action<Scene> SceneLoaded;
        private event Action<Scene> SceneUnloaded;

        public Scene ActiveScene => SceneManager.GetActiveScene();
        public string ActiveSceneName => ActiveScene.name;

        #region Life Cycle
        private void OnEnable()
        {
            ServiceLocator.Register(this);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            ServiceLocator.Unregister<SceneFacade>();
        }
        #endregion

        #region Registration
        public void BindSceneLoadStart(Action<Scene> e) => SceneLoadStarted += e;
        public void UnbindSceneLoadStart(Action<Scene> e) => SceneLoadStarted -= e;

        public void BindSceneLoaded(Action<Scene> e) => SceneLoaded += e;
        public void UnbindSceneLoaded(Action<Scene> e) => SceneLoaded -= e;

        public void BindSceneUnloaded(Action<Scene> e) => SceneUnloaded += e;
        public void UnbindSceneUnloaded(Action<Scene> e) => SceneUnloaded -= e;
        #endregion

        #region Load
        public void Load(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneLoadStarted?.Invoke(scene);

            SceneManager.LoadScene(scene.name, mode);
        }

        public async Task LoadAsync(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneLoadStarted?.Invoke(scene);

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene.name, mode);

            if (operation == null)
            {
                Debug.LogError($"Scene {scene.name} cannot be loaded.");
                return;
            }

            while (!operation.isDone)
                await Task.Yield();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneLoaded?.Invoke(scene);
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

        private void OnSceneUnloaded(Scene scene) 
        {
            SceneUnloaded?.Invoke(scene);
        }
        #endregion

        #region Reload
        public Task ReloadAsync() => LoadAsync(ActiveScene);
        #endregion

        #region Other API
        public Scene GetSceneByName(string sceneName)
            => SceneManager.GetSceneByName(sceneName);
        #endregion
    }
}