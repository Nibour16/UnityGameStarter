using System;
using UnityEditor;
using UnityEngine;

namespace UnityGameStarter.EditorUtilities.Installation
{
    [InitializeOnLoad]
    public static class ProjectSettingsInitializer
    {
        private static Action<UnityEditor.PackageManager.PackageInfo> _initializers;

        static ProjectSettingsInitializer() 
        {
            PackageInitializer.RequestInstall(ApplySettings);
        }

        private static void ApplySettings(UnityEditor.PackageManager.PackageInfo info)
        {
            if (_initializers == null) return;

            _initializers.Invoke(info);
            Debug.Log("Unity Game Starter: Project settings initialized.");
        }

        public static void BindToInitializers(Action<UnityEditor.PackageManager.PackageInfo> action) 
        {
            if (PackageInitializer.IsInitialized(out var info))
            {
                if (info == null) return;

                action(info);
                return;
            }

            _initializers += action;
        }
    }
}
