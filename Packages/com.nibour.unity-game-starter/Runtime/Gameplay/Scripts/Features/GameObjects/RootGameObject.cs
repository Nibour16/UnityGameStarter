using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.Object 
{
    /// <summary>
    /// A component that unbinds the game object from the parent object if have 
    /// </summary>
    public sealed class RootGameObject : OptionalSingleton<RootGameObject>
    {
        [SerializeField] private bool alwaysUnbindFromParent = false;
        [SerializeField] private bool dontDestroyOnLoad = false;
        [SerializeField] private bool isStatic = true;

        private Transform _originalParent;
        private Transform _lastUnbindedParent;

        protected override bool IsStatic => isStatic;

        /// <summary>
        /// Warning: If the original parent is destroyed, it may returns value equal to <see cref="LastUnbindedParent"/>
        /// </summary>
        public Transform OriginalParent => _originalParent;
        public Transform LastUnbindedParent => _lastUnbindedParent;

        protected override void Awake() 
        {
            base.Awake();

            UnbindFromParent();

            if (dontDestroyOnLoad)
                EnableDontDestroyOnLoad();
        }

        private void Update() 
        {
            if (alwaysUnbindFromParent)
                UnbindFromParent();
        }

        private void UnbindFromParent() 
        {
            if (transform.parent == null) return;

            _lastUnbindedParent = transform.parent;

            if (_originalParent == null)
                _originalParent = _lastUnbindedParent;

            transform.SetParent(null);
        }
    }   
}
