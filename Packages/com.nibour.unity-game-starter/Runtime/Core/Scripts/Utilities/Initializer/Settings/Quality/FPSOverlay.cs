using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.StarterSettings.Quality 
{
    [RuntimeSingleton(-400)]
    public sealed class FPSOverlay : Singleton<FPSOverlay>
    {
        private FPSSettings _settings;

        private float _deltaTime;
        private int _lastFontSize;
        private Color _lastFontColor;

        private GUIStyle _style;

        protected override void Awake()
        {
            base.Awake();
            _settings = StarterSettingsProvider.Get<FPSSettings>();
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            UpdateStyle();

            if (!_settings.ShowFPS) return;

            int fps = _deltaTime > 0 ? Mathf.RoundToInt(1f / _deltaTime) : 0;
            int targetFps = _settings.TargetFrameRate;
            var rect = _settings.OverlayRect;

            string limit = _settings.EnableVSync ? "VSync": targetFps.ToString();

            GUI.Label(rect, $"FPS: {fps} / {limit}", _style);
        }

        private void UpdateStyle()
        {
            if (!_settings) return;

            if (_style != null && _lastFontSize == _settings.FontSize && _lastFontColor == _settings.FontColor)
                return;

            _style = new GUIStyle(GUI.skin.label)
            {
                fontSize = _settings.FontSize
            };

            _style.normal.textColor = _settings.FontColor;

            _lastFontSize = _settings.FontSize;
            _lastFontColor = _settings.FontColor;
        }

        public void SetSettings(FPSSettings settings) 
        {
            if (_settings == settings) return;

            _settings = settings;
            _style = null;
        }
    }
}