using System.Collections.Generic;
using UnityEngine;

using UnityGameStarter.Events;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.UI 
{
    /// <summary>
    /// UI root that can collect all UI elements. This is where your UI will cling to
    /// </summary>
    /// <remarks>
    /// All UI elements that cling to the root must be children of the root canvas
    /// </remarks>
    [RequireComponent(typeof(EventListenerRegister))]
    public class UIRoot : OptionalSingleton<UIRoot>, IAutoEventListener
    {
        [SerializeField] private Canvas rootCanvas;

        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private bool isStatic = true;

        protected override bool IsStatic => isStatic;

        public Canvas RootCanvas 
        {
            get
            {
                if (rootCanvas == null)
                    Debug.LogError("UI Root: Root Canvas is not assigned");
                
                return rootCanvas;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            if (dontDestroyOnLoad)
                EnableDontDestroyOnLoad();
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (rootCanvas == null)
                rootCanvas = GetComponentInChildren<Canvas>(true);
        }
        #endif

        [EventListener]
        private void Collect(UICollectRequest request) 
        {
            var result = new Dictionary<string, RectTransform>();

            foreach (RectTransform rect in rootCanvas.transform)
            {
                if (rect.TryGetComponent<UIRoot>(out _)) continue;

                if (!result.TryAdd(rect.name, rect))
                    Debug.LogWarning($"UI Root: Duplicate UI name: {rect.name}");
            }

            request.Callback.Invoke(this, result);
        }
    }
}