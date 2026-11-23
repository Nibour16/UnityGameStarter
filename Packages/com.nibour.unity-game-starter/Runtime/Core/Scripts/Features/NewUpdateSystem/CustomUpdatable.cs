namespace UnityGameStarter.NewUpdateSystem 
{
    public enum UpdateType
    {
        NoUpdate,
        Normal,
        Fixed,
        Late,
        Lazy
    }

    public abstract class CustomUpdatable : LazyUpdatable
    {
        public abstract UpdateType SyncUpdateType { get; protected set; }

        private void Update() 
        {
            if (SyncUpdateType == UpdateType.Normal)
                OnUpdate();
        }

        private void FixedUpdate() 
        {
            if (SyncUpdateType == UpdateType.Fixed)
                OnUpdate();
        }

        private void LateUpdate() 
        {
            if (SyncUpdateType == UpdateType.Late)
                OnUpdate();
        }

        /// <summary>
        /// Warning: Recommended NOT override this method, override <see cref="OnUpdate"/> instead.
        /// </summary>
        /// <remarks>
        /// If overriding this method is intented, make sure to have the base of your overrided method in
        /// </remarks>
        protected override void LazyUpdate()
        {
            if (SyncUpdateType == UpdateType.Lazy)
                OnUpdate();
        }

        protected abstract void OnUpdate();
    }
}