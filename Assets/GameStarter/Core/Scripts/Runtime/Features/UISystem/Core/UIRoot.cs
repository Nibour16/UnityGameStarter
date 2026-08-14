using System.Collections.Generic;
using UnityEngine;

using UnityGameStarter.Events.EventManagement;

namespace UnityGameStarter.UI 
{
    [RequireComponent(typeof(EventListenerRegister))]
    public class UIRoot : MonoBehaviour, IAutoEventListener
    {
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private bool dontDestroyOnLoad = true;

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