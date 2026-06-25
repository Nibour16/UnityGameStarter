using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityGameStarter.EditorWindowUtilities.Data;

namespace UnityGameStarter.EditorWindowUtilities
{
    public abstract class EditorWindowWithInputs : EditorWindow
    {
        #region Config
        protected abstract string Title { get; }
        #endregion

        #region Definitions (immutable config)
        protected abstract Dictionary<string, ContentDefinition> Content { get; }
        private Dictionary<string, ContentDefinition> _content;
        private bool _contentInitialized;

        protected abstract Dictionary<string, ButtonDefinition> Buttons { get; }
        private Dictionary<string, ButtonDefinition> _buttons;
        private bool _buttonsInitialized;

        protected virtual float VerticalSpace => 10f;
        protected virtual Vector2 MinimumWindowSize => new(300f, 70f);
        #endregion

        #region Unity lifecycle
        private void OnEnable()
        {
            minSize = MinimumWindowSize;
        }
        #endregion

        #region Entry
        public static T ShowWindow<T>() where T : EditorWindowWithInputs
        {
            var window = GetWindow<T>();
            window.titleContent = new GUIContent(window.Title);

            return window;
        }
        #endregion

        #region GUI
        protected virtual void OnGUI()
        {
            DrawFields();
            DrawButtons();

            GUILayout.Space(VerticalSpace);
        }
        #endregion

        #region Drawing
        private void DrawFields()
        {
            InitializeIfNeededContent();

            foreach (var fd in _content)
            {
                fd.Value.value = EditorGUILayout.TextField(fd.Key, fd.Value.value);
            }
        }

        private void DrawButtons() 
        {
            InitializeIfNeededButtons();

            foreach (var button in _buttons)
            {
                bool enabled = button.Value.enabledCondition == null
                    || button.Value.enabledCondition(_content);

                using (new EditorGUI.DisabledScope(!enabled))
                {
                    if (GUILayout.Button(button.Key))
                    {
                        button.Value.onClicked?.Invoke(_content);

                        if (button.Value.closeOnClick)
                        {
                            Close();
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Definition Initialization
        private void InitializeIfNeededContent()
        {
            if (_contentInitialized) return;

            _content = Content ?? new Dictionary<string, ContentDefinition>();
            _contentInitialized = true;
        }

        private void InitializeIfNeededButtons()
        {
            if (_buttonsInitialized) return;

            _buttons = Buttons ?? new Dictionary<string, ButtonDefinition>();
            _buttonsInitialized = true;
        }
        #endregion
    }
}