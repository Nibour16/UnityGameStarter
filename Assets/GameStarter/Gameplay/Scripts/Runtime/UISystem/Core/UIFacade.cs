using System;
using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.UI 
{
    [RuntimeSingleton(-85)]
    public sealed class UIFacade : Singleton<UIFacade>
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

        #region API
        public void OpenUI(UIRoot root, string uiName, bool printLogIfNotFound = false)
        {
            if (!UIManager.Instance.TryGetRect(root, uiName, out var rect, printLogIfNotFound)) return;

            if (!IsValidUIElement(rect, uiName)) return;

            rect.gameObject.SetActive(true);

            if (rect.TryGetComponent<IUIComponent>(out var component))
                component.OnOpened();
        }

        public void OpenUI(string uiName, bool printLogIfNotFound) 
        {
            OpenUI(_defaultRoot, uiName, printLogIfNotFound);
        }

        public void OpenUI<T>(UIRoot root, T uiComponent, bool printLogIfNotFound = false) where T : Component
        {
            OpenUI(root, uiComponent.name, printLogIfNotFound);
        }

        public void OpenUI<T>(T uiComponent, bool printLogIfNotFound) where T : Component
        {
            OpenUI(_defaultRoot, uiComponent, printLogIfNotFound);
        }

        public void CloseUI(UIRoot root, string uiName, bool printLogIfNotFound = false)
        {
            if (!UIManager.Instance.TryGetRect(root, uiName, out var rect, printLogIfNotFound)) return;

            if (!IsValidUIElement(rect, uiName)) return;

            if (rect.TryGetComponent<IUIComponent>(out var component))
                component.OnClosed();

            rect.gameObject.SetActive(false);
        }

        public void CloseUI<T>(UIRoot root, T uiComponent, bool printLogIfNotFound = false) where T : Component
        {
            CloseUI(root, uiComponent.name, printLogIfNotFound);
        }

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