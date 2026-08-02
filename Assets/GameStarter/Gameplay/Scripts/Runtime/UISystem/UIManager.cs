using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using UnityGameStarter.CommonData;
using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.UI 
{
    public class UICollectRequest
    {
        public Action<Dictionary<string, RectTransform>> Callback { get; }

        public UICollectRequest(Action<Dictionary<string, RectTransform>> callback)
        {
            Callback = callback;
        }
    }

    [RuntimeSingleton]
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<string, RectTransform> _uiComponents = new(StringComparer.Ordinal);

        private InitializationState _initializeState = InitializationState.Uninitialized;
        public InitializationState InitializeState => _initializeState;

        #region Initialization
        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();

            SceneFacade.Instance.BindSceneLoaded(Initialize);
            SceneFacade.Instance.BindSceneUnloaded(Deinitialize);
        }

        private void Initialize(Scene e) 
        {
            if (_initializeState != InitializationState.Uninitialized) return;

            if (UIRoot.Instance == null) return;
            
            _initializeState = InitializationState.Initializing;
            EventManager.Instance.Publish(new UICollectRequest(OnCollected));

            foreach (var ui in _uiComponents) 
            {
                if (ui.Value.TryGetComponent<IUIComponent>(out var component))
                    component.Init();
            }

            _initializeState = InitializationState.Initialized;
        }

        private void Deinitialize(Scene e) 
        {
            if (_uiComponents != null) 
            {
                foreach (var ui in _uiComponents.Values)
                {
                    if (ui.TryGetComponent<IUIComponent>(out var component))
                        component.Deinit();
                }
                _uiComponents.Clear();
            }
            
            _initializeState = InitializationState.Uninitialized;
        }

        private void OnCollected(Dictionary<string, RectTransform> data) 
        {
            _uiComponents = data;
        }
        #endregion

        #region API
        public void OpenUI(string uiName) 
        {
            if (!TryGetRect(uiName, out var rect)) return;

            if (!IsValidUIElement(rect, uiName)) return;

            rect.gameObject.SetActive(true);

            if (rect.TryGetComponent<IUIComponent>(out var component))
                component.OnOpened();
        }

        public void OpenUI<T>(T uiComponent) where T : Component
        {
            OpenUI(uiComponent.name);
        }

        public void CloseUI(string uiName) 
        {
            if (!TryGetRect(uiName, out var rect)) return;

            if (!IsValidUIElement(rect, uiName)) return;

            if (rect.TryGetComponent<IUIComponent>(out var component))
                component.OnClosed();

            rect.gameObject.SetActive(false);
        }

        public void CloseUI<T>(T uiComponent) where T : Component
        {
            CloseUI(uiComponent.name);
        }

        public bool TryGetUI<T>(string uiName, out T ui) where T : Component
        {
            if (!TryGetRect(uiName, out var rect)) 
            {
                ui = null;
                return false;
            }

            if (!rect.TryGetComponent(out ui)) 
            {
                ui = null;
                return false;
            }

            return true;
        }

        public T GetUI<T>(string uiName) where T : Component
        {
            if (!TryGetUI(uiName, out T ui))
                throw new Exception($"UI Manager: the ui component {uiName} is not found");

            return ui;
        }
        #endregion

        #region Private Methods
        private bool TryGetRect(string uiName, out RectTransform rect, bool printDebug = true) 
        {
            if (!_uiComponents.TryGetValue(uiName, out rect))
            {
                rect = null;

                if (printDebug)
                    Debug.LogWarning($"UI Manager: the ui component {uiName} is not found");
                
                return false;
            }

            return true;
        }

        private bool IsValidUIElement(RectTransform rect, string uiName) 
        {
            if (rect.IsCanvasRoot())
            {
                Debug.LogError($"UI Manager: the UI Component {uiName} is a Canvas Root");
                return false;
            }

            if (!rect.IsUI())
            {
                Debug.LogError($"UI Manager: the UI Component {uiName} is not a valid UI");
                return false;
            }

            return true;
        }
        #endregion
    }
}