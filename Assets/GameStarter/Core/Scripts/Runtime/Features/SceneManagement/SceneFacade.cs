using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.ServiceLocatorPattern;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.SceneManagement
{
    [RuntimeSingleton(-200)]
    public class SceneFacade : Singleton<SceneFacade>, ISceneService
    {
        private event Action<Scene, LoadSceneMode> SceneLoadStarted;
        private event Action<Scene, LoadSceneMode> SceneLoaded;
        private event Action<Scene> SceneUnloadStarted;
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
        public void BindSceneLoadStart(Action<Scene, LoadSceneMode> e) => SceneLoadStarted += e;
        public void UnbindSceneLoadStart(Action<Scene, LoadSceneMode> e) => SceneLoadStarted -= e;

        public void BindSceneLoaded(Action<Scene, LoadSceneMode> e) => SceneLoaded += e;
        public void UnbindSceneLoaded(Action<Scene, LoadSceneMode> e) => SceneLoaded -= e;

        public void BindSceneUnloadStart(Action<Scene> e) => SceneUnloadStarted += e;
        public void UnbindSceneUnloadStart(Action<Scene> e) => SceneUnloadStarted -= e;

        public void BindSceneUnloaded(Action<Scene> e) => SceneUnloaded += e;
        public void UnbindSceneUnloaded(Action<Scene> e) => SceneUnloaded -= e;
        #endregion

        #region Load
        public void Load(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!scene.IsValid())
            {
                Debug.LogError(
                    $"Cannot load scene: Scene is invalid. " +
                    $"name=[{scene.name}], buildIndex=[{scene.buildIndex}], path=[{scene.path}]");

                return;
            }

            if (string.IsNullOrEmpty(scene.name))
            {
                Debug.LogError("Cannot load scene: Scene name is empty.");
                return;
            }

            SceneLoadStarted?.Invoke(scene, mode);
            SceneManager.LoadScene(scene.name, mode);
        }

        public bool Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, bool printError = false)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                if (printError)
                    Debug.LogError("Cannot load scene: sceneName is null or empty.");
                
                return false;
            }

            SceneLoadStarted?.Invoke(GetSceneByName(sceneName), mode);
            SceneManager.LoadScene(sceneName, mode);
            return true;
        }

        public async Task LoadAsync(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneLoadStarted?.Invoke(scene, mode);

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene.name, mode);

            if (operation == null)
            {
                Debug.LogError($"Scene {scene.name} cannot be loaded.");
                return;
            }

            while (!operation.isDone)
                await Task.Yield();
        }

        public async Task LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneLoadStarted?.Invoke(GetSceneByName(sceneName), mode);

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
            SceneLoaded?.Invoke(scene, mode);
        }
        #endregion

        #region Unload
        public async Task UnloadAsync(Scene scene)
        {
            SceneUnloadStarted?.Invoke(scene);

            AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);

            if (operation == null)
                return;

            while (!operation.isDone)
                await Task.Yield();
        }

        public async Task UnloadAsync(string sceneName)
        {
            SceneUnloadStarted?.Invoke(GetSceneByName(sceneName));

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
        public void Reload(LoadSceneMode mode = LoadSceneMode.Single) => Load(ActiveScene, mode);
        public Task ReloadAsync(LoadSceneMode mode = LoadSceneMode.Single) => LoadAsync(ActiveScene, mode);
        #endregion

        #region Other API
        public Scene GetSceneByName(string sceneName)
            => SceneManager.GetSceneByName(sceneName);

        public bool IsValidScene(Scene scene)
            => scene.IsValid();

        public bool IsValidScene(string sceneName)
            => IsValidScene(GetSceneByName(sceneName));
        #endregion
    }
}