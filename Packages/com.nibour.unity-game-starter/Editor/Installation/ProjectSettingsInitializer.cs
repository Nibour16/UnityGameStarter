using System;
using System.Linq;
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

            foreach (Action<UnityEditor.PackageManager.PackageInfo> initializer 
                in _initializers.GetInvocationList().Cast<Action<UnityEditor.PackageManager.PackageInfo>>())
            {
                try
                {
                    initializer.Invoke(info);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
            Debug.Log("[Unity Game Starter] Project settings initialized.");
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
