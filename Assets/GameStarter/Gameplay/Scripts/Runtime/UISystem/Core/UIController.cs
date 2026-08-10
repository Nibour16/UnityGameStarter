using System;
using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.UI 
{
    [RuntimeSingleton(-85)]
    public sealed class UIController : Singleton<UIController>
    {
        private UIRoot _defaultRoot;

        public UIRoot DefaultRoot
        {
            get
            {
                if (_defaultRoot == null)
                    _defaultRoot = FindAnyObjectByType<UIRoot>();

                return _defaultRoot;
            }
        }

        private RectTransform _lastOpenedUI;
        private RectTransform _lastClosedUI;

        public RectTransform LastOpenedUI => _lastOpenedUI;
        public RectTransform LastClosedUI => _lastClosedUI;

        #region API
        public bool OpenUI(UIRoot root, string uiName, bool printLogIfNotFound = false)
        {
            if (!UIManager.Instance.TryGetRect(root, uiName, out var rect, printLogIfNotFound)) return false;

            if (!IsValidUIElement(rect, uiName)) return false;

            rect.gameObject.SetActive(true);

            _lastOpenedUI = rect;

            if (rect.TryGetComponent<IUIComponent>(out var component))
                component.OnOpened();

            return true;
        }

        public bool OpenUI(string uiName, bool printLogIfNotFound = false) 
            => OpenUI(DefaultRoot, uiName, printLogIfNotFound);

        public bool OpenUI<T>(UIRoot root, T uiComponent, bool printLogIfNotFound = false) where T : Component
        {
            if (!uiComponent) return false;
            
            return OpenUI(root, uiComponent.name, printLogIfNotFound);
        }

        public bool OpenUI<T>(T uiComponent, bool printLogIfNotFound = false) where T : Component
            => OpenUI(DefaultRoot, uiComponent, printLogIfNotFound);
        
        public bool CloseUI(UIRoot root, string uiName, bool printLogIfNotFound = false)
        {
            if (!UIManager.Instance.TryGetRect(root, uiName, out var rect, printLogIfNotFound)) return false;

            if (!IsValidUIElement(rect, uiName)) return false;

            if (rect.TryGetComponent<IUIComponent>(out var component))
                component.OnClosed();

            rect.gameObject.SetActive(false);

            _lastClosedUI = rect;

            return true;
        }

        public bool CloseUI(string uiName, bool printLogIfNotFound = false)
            => CloseUI(DefaultRoot, uiName, printLogIfNotFound);

        public bool CloseUI<T>(UIRoot root, T uiComponent, bool printLogIfNotFound = false) where T : Component
        {
            if (!uiComponent) return false;

            return CloseUI(root, uiComponent.name, printLogIfNotFound);
        }

        public bool CloseUI<T>(T uiComponent, bool printLogIfNotFound = false) where T : Component
            => CloseUI(DefaultRoot, uiComponent, printLogIfNotFound);

        public bool TryGetUI<T>(UIRoot root, string uiName, out T ui, bool printLogIfNotFound = false) where T : Component
        {
            if (!UIManager.Instance.TryGetRect(root, uiName, out var rect, printLogIfNotFound))
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

        public T GetUI<T>(UIRoot root, string uiName) where T : Component
        {
            if (!TryGetUI(root, uiName, out T ui))
                throw new Exception($"UI Manager: the ui component {uiName} is not found");

            return ui;
        }
        #endregion

        #region Private Methods
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