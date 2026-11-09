using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.CommonData;
using UnityGameStarter.DataStructure;
using UnityGameStarter.Events;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.UI 
{
    /// <summary>
    /// An event class for requesting UI Manager through Unity Game Starter's Event System
    /// </summary>
    /// <remarks>
    /// Unity Game Starter's Event System is an appllication of Event Bus Pattern and Unity Events, 
    /// different from Unity's EventSystem. See <see cref="EventManager"/>
    /// </remarks>
    public sealed class UICollectRequest
    {
        public Action<UIRoot, Dictionary<string, RectTransform>> Callback { get; }

        public UICollectRequest(Action<UIRoot, Dictionary<string, RectTransform>> callback)
        {
            Callback = callback;
        }
    }

    /// <summary>
    /// Manager that collects all UI root with their collected UI elements like menus and/or huds
    /// </summary>
    [RuntimeSingleton(-90)]
    public sealed class UIManager : Singleton<UIManager>
    {
        private readonly Dictionary<UIRoot, Dictionary<string, RectTransform>> _ui = new();
        private readonly Dictionary<Scene, InitializationState> _initializeStates = new();

        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();

            SceneFacade.Instance.BindSceneLoaded(Initialize);
            SceneFacade.Instance.BindSceneUnloadStart(Deinitialize);
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
                InitializeRoot(root);

            _initializeStates[e] = InitializationState.Initialized;
        }

        private void Deinitialize(Scene e) 
        {
            var removeRoots = _ui.Keys.Where(
                root => root != null && root.gameObject != null &&
                root.gameObject.scene.IsValid() && root.gameObject.scene == e).ToArray();

            foreach (var root in removeRoots)
            {
                foreach (var ui in _ui[root].Values)
                {
                    var uiComps = ui.GetComponents<IUIComponent>();

                    foreach (var comp in uiComps)
                        comp.Deinit();
                }

                _ui.Remove(root);
            }

            _initializeStates.Remove(e);
        }
        #endregion

        #region Private Methods
        private void InitializeRoot(UIRoot root)
        {
            if (!_ui.TryGetValue(root, out var collection))
                return;

            foreach (var ui in collection.Values)
            {
                var uiComps = ui.GetComponents<IUIComponent>();

                foreach (var comp in uiComps)
                    comp.Init();
            }
        }

        private void OnCollected(UIRoot root, Dictionary<string, RectTransform> data) 
        {
            if (!_ui.TryGetValue(root, out var collection))
            {
                collection = new(StringComparer.Ordinal);
                _ui.Add(root, collection);
            }

            collection.AddRangeSafe(data);
        }
        #endregion

        #region API
        public bool TryGetRect(UIRoot root, string uiName, out RectTransform rect, bool printDebug = false)
        {
            if (!_ui.TryGetValue(root, out var components))
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