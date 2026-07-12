using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.StarterSettings
{
    public interface IStarterSetting { }

    public static class StarterSettingsProvider
    {
        private static readonly Dictionary<Type, ScriptableObject> cache = new();
        private static bool _initialized;

        public static void Initialize(StarterSettingsRoot root)
        {
            if (_initialized)
            {
                Debug.LogWarning("SettingsProvider already initialized.");
                return;
            }

            foreach (var setting in root.Settings)
            {
                if (setting is not IStarterSetting)
                {
                    Debug.LogError($"{setting.name} is not IStarterSetting.");
                    continue;
                }

                var type = setting.GetType();

                if (cache.ContainsKey(type))
                {
                    Debug.LogError($"Duplicate setting type: {type}");
                    continue;
                }

                cache.Add(type, setting);
            }

            _initialized = true;
        }

        public static T Get<T>() where T : ScriptableObject, IStarterSetting
        {
            if (!cache.TryGetValue(typeof(T), out var value))
            {
                Debug.LogError($"Setting {typeof(T)} not registered.");
                return null;
            }

            return (T)value;
        }
    }
}