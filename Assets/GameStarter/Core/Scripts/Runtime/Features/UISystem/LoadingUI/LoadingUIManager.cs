using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;
using UnityGameStarter.StarterSettings;
using UnityGameStarter.StarterSettings.Quality;

namespace UnityGameStarter.UI.Loading 
{
    [RuntimeSingleton(-80)]
    public sealed class LoadingUIManager : Singleton<LoadingUIManager>
    {
        private UIRoot _uiRoot;
        private string _uiName;

        private int _loadingCount = 0;

        protected override void Awake()
        {
            base.Awake();

            var settings = StarterSettingsProvider.Get<LoadingUISettings>();

            if (!settings) 
            {
                Debug.LogWarning("Loading UI Settings not found");
                return;
            }

            var rootPrefab = settings.LoadingUIRoot;

            if (!rootPrefab)
            {
                Debug.LogWarning("Loading UI Root Prefab not found");
                return;
            }

            _uiRoot = Instantiate(rootPrefab);
            _uiName = settings.LoadingUIName;
        }

        private void OnEnable() 
        {
            if (!SceneFacade.TryGetInstance(out var instance)) return;

            instance.BindSceneLoadStart(OpenLoadingUI);
            instance.BindSceneUnloadStart(OpenLoadingUI);

            instance.BindSceneLoaded(CloseLoadingUI);
            instance.BindSceneUnloaded(CloseLoadingUI);
        }

        private void OnDisable() 
        {
            if (!SceneFacade.TryGetInstance(out var instance)) return;

            instance.UnbindSceneLoadStart(OpenLoadingUI);
            instance.UnbindSceneUnloadStart(OpenLoadingUI);

            instance.UnbindSceneLoaded(CloseLoadingUI);
            instance.UnbindSceneUnloaded(CloseLoadingUI);
        }

        private void OpenLoadingUI(Scene e, LoadSceneMode mode) => OpenLoadingUI();
        private void OpenLoadingUI(Scene e) => OpenLoadingUI();

        private void CloseLoadingUI(Scene e, LoadSceneMode mode) => CloseLoadingUI();
        private void CloseLoadingUI(Scene e) => CloseLoadingUI();

        private void OpenLoadingUI() 
        {
            if (_uiRoot == null || _uiName == "") return;
            
            _loadingCount++;

            if (_loadingCount == 1)
                UIController.Instance.OpenUI(_uiRoot, _uiName);
        }

        private void CloseLoadingUI()
        {
            if (_uiRoot == null || _uiName == "") return;

            _loadingCount = Mathf.Max(0, _loadingCount - 1);

            if (_loadingCount == 0)
                UIController.Instance.CloseUI(_uiRoot, _uiName);
        }
    }
}