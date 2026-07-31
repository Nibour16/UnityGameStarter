using UnityEngine;

namespace UnityGameStarter.StarterSettings.Quality
{
    [CreateAssetMenu(fileName = "NewFPSSetting", 
        menuName = "Scriptable Objects/Unity Game Starter/Quality/FPS Setting")]
    public class FPSSettings : ScriptableObject, IStarterSetting
    {
        [Header("Performance")]
        [SerializeField, Min(10)] private int targetFrameRate = 60;
        [SerializeField] private bool enableVSync = false;

        [Header("Overlay")]
        [SerializeField] private bool showFPS = false;
        [SerializeField] private Rect overlayRect = new(10, 10, 200, 40);
        [SerializeField] private int fontSize = 18;
        [SerializeField] private Color fontColor = Color.white;

        public int TargetFrameRate => targetFrameRate;
        public bool EnableVSync => enableVSync;

        public bool ShowFPS => showFPS;
        public Rect OverlayRect => overlayRect;
        public int FontSize => fontSize;
        public Color FontColor => fontColor;
    }
}