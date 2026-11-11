using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.SceneManagement
{
    /// <summary>
    /// Scene facade where methods that need to be executed during scene load/unload to register/unregister
    /// Can also offer methods to load/unload scenes, includes async tasks
    /// </summary>
    [RuntimeSingleton(-200)]
    public class SceneFacade : Singleton<SceneFacade>
    {
        #region Track of scene load events
        private event Action<Scene, LoadSceneMode> SceneLoadStarted;
        private event Action<Scene, LoadSceneMode> SceneLoaded;
        private event Action<Scene> SceneUnloadStarted;
        private event Action<Scene> SceneUnloaded;
        #endregion

        #region Scene Getters
        public Scene ActiveScene => SceneManager.GetActiveScene();
        public string ActiveSceneName => ActiveScene.name;
        public Scene GetSceneByName(string sceneName) => SceneManager.GetSceneByName(sceneName);
        #endregion

        #region Life Cycle
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
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
        public bool Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!IsSceneNameValid(sceneName)) return false;

            SceneLoadStarted?.Invoke(GetSceneByName(sceneName), mode);
            SceneManager.LoadScene(sceneName, mode);
            return true;
        }

        public bool Load(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!IsSceneValid(scene)) return false;

            return Load(scene.name, mode);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => SceneLoaded?.Invoke(scene, mode);
        #endregion

        #region Load Async
        public Task LoadAsync(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!IsSceneValid(scene)) return Task.CompletedTask;

            return LoadAsync(scene.name, mode);
        }

        public async Task LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!IsSceneNameValid(sceneName)) return;

            SceneLoadStarted?.Invoke(GetSceneByName(sceneName), mode);

            if (!TryGetLoadSceneAsyncOperation(sceneName, out var operation, mode)) return;

            while (!operation.isDone)
                await Task.Yield();
        }

        public IEnumerator DoLoadAsyncTask(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) 
        {
            Task task = LoadAsync(sceneName, mode);

            while (!task.IsCompleted) yield return null;

            ValidateTaskStatus(task);
        }

        public IEnumerator DoLoadAsyncTask(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
            => DoLoadAsyncTask(scene.name, mode);

        #endregion

        #region Unload
        public Task UnloadAsync(Scene scene)
        {
            if (!IsSceneValid(scene)) return Task.CompletedTask;

            return UnloadAsync(scene.name);
        }

        public async Task UnloadAsync(string sceneName)
        {
            if (!IsSceneNameValid(sceneName)) return;
            
            SceneUnloadStarted?.Invoke(GetSceneByName(sceneName));

            if (!TryGetUnloadSceneAsyncOperation(sceneName, out var operation)) return;

            while (!operation.isDone)
                await Task.Yield();
        }

        public IEnumerator DoUnloadAsyncTask(string sceneName)
        {
            Task task = UnloadAsync(sceneName);

            while (!task.IsCompleted) yield return null;

            ValidateTaskStatus(task);
        }

        public IEnumerator DoUnloadAsyncTask(Scene scene) => DoUnloadAsyncTask(scene.name);

        private void OnSceneUnloaded(Scene scene) => SceneUnloaded?.Invoke(scene);
        #endregion

        #region Reload
        public void Reload(LoadSceneMode mode = LoadSceneMode.Single) => Load(ActiveScene, mode);
        public Task ReloadAsync(LoadSceneMode mode = LoadSceneMode.Single) => LoadAsync(ActiveScene, mode);
        #endregion

        #region Async Operation Handlers
        private bool TryGetLoadSceneAsyncOperation(string sceneName, out AsyncOperation operation,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            operation = SceneManager.LoadSceneAsync(sceneName, mode);

            return operation != null;
        }

        private bool TryGetUnloadSceneAsyncOperation(string sceneName, out AsyncOperation operation)
        {
            operation = SceneManager.UnloadSceneAsync(sceneName);

            return operation != null;
        }
        #endregion

        #region Validation
        public bool IsSceneValid(Scene scene)
        {
            if (!scene.IsValid())
            {
                Debug.LogError($"Invalid scene: name=[{scene.name}], " +
                    $"buildIndex=[{scene.buildIndex}], path=[{scene.path}]", this);

                return false;
            }

            return true;
        }

        public bool IsSceneNameValid(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is empty.", this);
                return false;
            }

            return true;
        }

        private void ValidateTaskStatus(Task task)
        {
            if (task.IsFaulted)
                Debug.LogException(task.Exception, this);
            else if (task.IsCanceled)
                Debug.LogWarning("Scene task was canceled.", this);
        }
        #endregion
    }
}