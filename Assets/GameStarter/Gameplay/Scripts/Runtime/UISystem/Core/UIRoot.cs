using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

using UnityGameStarter.Events.EventManagement;

namespace UnityGameStarter.Gameplay.UI 
{
    [RequireComponent(typeof(EventListenerRegister))]
    public class UIRoot : MonoBehaviour, IAutoEventListener
    {
        [SerializeField] private Canvas rootCanvas;

        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private bool useMarker = false;

        public Canvas RootCanvas 
        {
            get
            {
                if (rootCanvas == null)
                    Debug.LogError("UI Root: Root Canvas is not assigned");
                
                return rootCanvas;
            }
        }

        private void Awake()
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
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

            RectTransform[] rects;
            if (useMarker) 
            {
                var elements = FindObjectsByType<UIElement>(FindObjectsInactive.Include);
                rects = new RectTransform[elements.Length];

                for (int i = 0; i < elements.Length; i++)
                    rects[i] = elements[i].Rect;
            }
            else
                rects = FindObjectsByType<RectTransform>(FindObjectsInactive.Include);

            foreach (var rect in rects)
            {
                if (rect.TryGetComponent<UIRoot>(out _)) continue;

                if (rect.IsCanvasRoot()) continue;
                
                if (!rect.IsUI()) continue;

                if (!result.TryAdd(rect.name, rect))
                    Debug.LogWarning($"UI Root: Duplicate UI name: {rect.name}");
            }

            request.Callback.Invoke(this, result);
        }
    }
}