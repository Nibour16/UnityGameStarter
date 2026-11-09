using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.Object 
{
    public sealed class RootGameObject : OptionalSingleton<RootGameObject>
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        [SerializeField] private bool isStatic = true;

        private Transform _originalParent;

        protected override bool IsStatic => isStatic;
        public Transform OriginalParent => _originalParent;

        protected override void Awake() 
        {
            base.Awake();
            
            _originalParent = transform.parent;
            transform.SetParent(null);

            if (dontDestroyOnLoad)
                EnableDontDestroyOnLoad();
        }
    }   
}
