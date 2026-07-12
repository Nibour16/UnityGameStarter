using UnityEngine;
using UnityGameStarter.StarterSettings;

namespace UnityGameStarter.Quality 
{
    [CreateAssetMenu(fileName = "NewFPSSetting", 
        menuName = "Scriptable Objects/Unity Game Starter/Quality/FPS Setting")]
    public class FPSSettings : ScriptableObject, IStarterSetting
    {
        [SerializeField, Min(30)] private int targetFrameRate = 60;

        [Header("Performance")]
        [SerializeField] private bool enableVSync = false;

        public bool EnableVSync => enableVSync;
        public int TargetFrameRate => targetFrameRate;
    }
}