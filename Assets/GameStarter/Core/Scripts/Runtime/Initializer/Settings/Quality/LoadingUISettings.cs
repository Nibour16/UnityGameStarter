using UnityEngine;
using UnityGameStarter.UI;

namespace UnityGameStarter.StarterSettings.Quality
{
    [CreateAssetMenu(fileName = "NewLoadingUISettings",
    menuName = "Scriptable Objects/Unity Game Starter/Quality/Loading UI Setting")]
    public sealed class LoadingUISettings : ScriptableObject, IStarterSetting
    {
        [SerializeField] private UIRoot loadingUIRoot;
        [SerializeField] private bool forceDontDestroyOnLoad = true;
        [SerializeField] private string loadingUIName = "LoadingUI";

        public UIRoot LoadingUIRoot => loadingUIRoot;
        public bool ForceDontDestroyOnLoad => forceDontDestroyOnLoad;
        public string LoadingUIName => loadingUIName;
    }
}

