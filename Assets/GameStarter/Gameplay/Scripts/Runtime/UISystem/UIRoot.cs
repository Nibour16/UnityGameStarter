using System.Collections.Generic;
using UnityEngine;

using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.UI 
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIElement : MonoBehaviour 
    {
        public RectTransform Rect => (RectTransform)transform;
    }

    [RequireComponent(typeof(EventListenerRegister))]
    public sealed class UIRoot : Singleton<UIRoot>, IAutoEventListener
    {
        [SerializeField] private bool dontDestroyOnLoad = true;

        [Header("Advanced")]
        [SerializeField] private bool useMarker = false;

        protected override void Awake()
        {
            base.Awake();

            if (dontDestroyOnLoad) EnableDontDestroyOnLoad();
        }

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
                if (rect.IsCanvasRoot()) continue;
                
                if (!rect.IsUI()) continue;

                if (!result.TryAdd(rect.name, rect))
                    Debug.LogWarning($"UI Root: Duplicate UI name: {rect.name}");
            }

            request.Callback.Invoke(result);
        }
    }
}