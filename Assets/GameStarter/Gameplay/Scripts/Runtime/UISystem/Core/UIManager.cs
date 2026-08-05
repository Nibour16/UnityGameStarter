using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.CommonData;
using UnityGameStarter.DataStructure;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.UI 
{
    public sealed class UICollectRequest
    {
        public Action<UIRoot, Dictionary<string, RectTransform>> Callback { get; }

        public UICollectRequest(Action<UIRoot, Dictionary<string, RectTransform>> callback)
        {
            Callback = callback;
        }
    }

    [RuntimeSingleton(-90)]
    public sealed class UIManager : Singleton<UIManager>
    {
        private readonly Dictionary<UIRoot, Dictionary<string, RectTransform>> _uiComponents = new();

        private readonly Dictionary<Scene, InitializationState> _initializeStates = new();
        public Dictionary<Scene, InitializationState> InitializeStates => _initializeStates;

        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();

            SceneFacade.Instance.BindSceneLoaded(Initialize);
            SceneFacade.Instance.BindSceneUnloaded(Deinitialize);
        }

        private void Start() 
        {
            Initialize(SceneManager.GetActiveScene());
        }

        private void Initialize(Scene e, LoadSceneMode mode = LoadSceneMode.Single) 
        {
            if (_initializeStates.TryGetValue(e, out var state)) 
            {
                if (state != InitializationState.Uninitialized) return;

                _initializeStates[e] = InitializationState.Initializing;
            }
            else
                _initializeStates.Add(e, InitializationState.Initializing);

            HashSet<UIRoot> collectedRoots = new();

            EventManager.Instance.Publish(new UICollectRequest((root, data) =>
            {
                OnCollected(root, data);
                collectedRoots.Add(root);
            }));

            foreach (var root in collectedRoots)
            {
                InitializeRoot(root);
            }

            _initializeStates[e] = InitializationState.Initialized;
        }

        private void Deinitialize(Scene e) 
        {
            var removeRoots = _uiComponents.Keys.Where(root => root.gameObject.scene == e)
                .ToArray();

            foreach (var root in removeRoots)
            {
                foreach (var ui in _uiComponents[root].Values)
                {
                    if (ui.TryGetComponent<IUIComponent>(out var component))
                        component.Deinit();
                }

                _uiComponents.Remove(root);
            }

            _initializeStates.Remove(e);
        }
        #endregion

        #region Private Methods
        private void InitializeRoot(UIRoot root)
        {
            if (!_uiComponents.TryGetValue(root, out var collection))
                return;

            foreach (var ui in collection.Values)
            {
                if (ui.TryGetComponent<IUIComponent>(out var component))
                    component.Init();
            }
        }

        private void OnCollected(UIRoot root, Dictionary<string, RectTransform> data) 
        {
            if (!_uiComponents.TryGetValue(root, out var collection))
            {
                collection = new(StringComparer.Ordinal);
                _uiComponents.Add(root, collection);
            }

            collection.AddRangeSafe(data);
        }
        #endregion

        #region API
        public bool TryGetRect(UIRoot root, string uiName, out RectTransform rect, bool printDebug = false)
        {
            if (!_uiComponents.TryGetValue(root, out var components))
            {
                rect = null;

                if (printDebug)
                    Debug.LogWarning($"UI Manager: the ui root '{root.name}' is not found");

                return false;
            }

            if (!components.TryGetValue(uiName, out rect))
            {
                rect = null;

                if (printDebug)
                    Debug.LogWarning($"UI Manager: the ui component '{uiName}' is not found");

                return false;
            }

            return true;
        }
        #endregion
    }
}