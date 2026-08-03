using System.Collections.Generic;
using UnityEngine;

using UnityGameStarter.EventSystem.EventManagement;

namespace UnityGameStarter.Gameplay.UI 
{
    [RequireComponent(typeof(EventListenerRegister))]
    public class UIRoot : MonoBehaviour, IAutoEventListener
    {
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private bool useMarker = false;

        private void Awake()
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
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