using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.StarterSettings
{
    [CreateAssetMenu(fileName = "StarterSettingsRoot", 
        menuName = "Scriptable Objects/Unity Game Starter/Advanced/Starter Settings Root")]
    public sealed class StarterSettingsRoot : ScriptableObject
    {
        [SerializeField]
        private List<ScriptableObject> settings = new();

        public IReadOnlyList<ScriptableObject> Settings => settings;
    }
}