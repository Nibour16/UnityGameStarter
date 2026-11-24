using System;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.EditorUtilities.Installation
{
    [InitializeOnLoad]
    static class PackageInitializer
    {
        private static Action<UnityEditor.PackageManager.PackageInfo> _installRequests;

        private const string _initializationBoolTag = "UnityGameStarter.Initialized";
        private const string _versionPrefsKey = "UnityGameStarter.PackageVersion";

        private static UnityEditor.PackageManager.PackageInfo PackageInfo 
        {
            get 
            {
                var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(
                    typeof(PackageInitializer).Assembly);

                if (packageInfo == null)
                    Debug.LogError("Unable to locate Unity Game Starter package.");

                return packageInfo;
            }
        }

        static PackageInitializer()
        {
            EditorApplication.delayCall += OnInstalled;
        }

        internal static void RequestInstall(Action<UnityEditor.PackageManager.PackageInfo> action) 
        { 
            if (IsInitialized(out var info)) 
            {
                if (info == null) return;

                action(info);
                return;
            }
            
            _installRequests += action; 
        }

        public static bool IsInitialized() => EditorPrefs.GetBool(_initializationBoolTag);
        public static bool IsInitialized(out UnityEditor.PackageManager.PackageInfo info) 
        {
            info = PackageInfo;
            return IsInitialized();
        }

        public static bool IsSameVersion(out string currentVersion)
        {
            currentVersion = null;
            var packageInfo = PackageInfo;

            if (packageInfo != null)
                currentVersion = packageInfo.version;

            string installedVersion = EditorPrefs.GetString(_versionPrefsKey, string.Empty);

            return installedVersion == currentVersion;
        }

        private static void OnInstalled() 
        {
            if (EditorPrefs.GetBool(_initializationBoolTag, false)) return;
            
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            if (IsSameVersion(out string currentVersion)) return;

            if (currentVersion == null) return;

            _installRequests?.Invoke(PackageInfo);

            EditorPrefs.SetBool(_initializationBoolTag, true);
            EditorPrefs.SetString(_versionPrefsKey, currentVersion);
            _installRequests = null;
        }
    }
}
