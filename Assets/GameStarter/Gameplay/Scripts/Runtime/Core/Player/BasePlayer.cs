using UnityEngine;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Math.TransformStatics;
using static UnityGameStarter.Gameplay.CharacterMovement.MovementLibrary;
using static UnityGameStarter.Math.TransformStatics.VectorLibrary;

namespace UnityGameStarter.Gameplay.PlayerSystem 
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(EventListenerRegister))]
    public abstract class BasePlayer : MonoBehaviour, IAutoEventListener
    {
        #region Settings
        [Header("Movement")]
        [SerializeField, Min(0.01f)] protected float moveSpeed = 4f;

        [Header("Advanced")]
        [SerializeField] private bool restoreVelocityAfterPause = true;
        #endregion

        #region References
        private PlayerController _playerController;
        public PlayerController PlayerController => _playerController;
        #endregion

        #region Velocity
        private Vector3 _playerVelocity;
        protected Vector3 PlayerVelocity => _playerVelocity;
        protected Vector2 PlayerVelocity2D => _playerVelocity.XZToVector2();

        private Vector3 _cachedVelocity = Vector3.zero;
        #endregion

        #region Overridable Properties
        protected virtual bool EnableMotion => true;
        protected virtual bool UseFixedUpdateMove => false;

        protected abstract Transform ControlSource { get; }
        protected abstract bool UseControlRotation { get; }
        protected abstract Vector2 MoveInput { get; }
        protected abstract bool PauseInput { get; }
        #endregion

        #region Velocity API
        public void SetPlayerVelocity(Vector3 newVelocity) 
            => _playerVelocity = newVelocity;

        public void SetPlayerVelocity2D(Vector2 newVelocity)
            => _playerVelocity = newVelocity.ToVector3XZ();

        public void SetPlayerVelocityParam(VectorParam param, float value)
            => SetVector3Param(ref _playerVelocity, param, value);

        public void AddPlayerVelocityParam(VectorParam param, float value)
            => AddVector3Param(ref _playerVelocity, param, value);
        #endregion

        #region Life Cycle
        private void Awake() 
        {
            _playerController = GetComponent<PlayerController>();
            OnInitialized();
        }

        protected virtual void OnInitialized() { }

        private void Update() 
        {
            OnUpdate();

            if (EnableMotion) 
            {
                if (!UseFixedUpdateMove)
                    HandleMoveVelocity();

                OnMotionUpdate();
            }

            if (PauseInput)
                _playerController.SetPause();
        }

        private void FixedUpdate() 
        {
            OnFixedUpdate();

            if (EnableMotion) 
            {
                if (UseFixedUpdateMove)
                    HandleMoveVelocity();

                OnMotionFixedUpdate();
            }
        }

        protected virtual void OnUpdate() { }
        protected virtual void OnMotionUpdate() { }

        protected virtual void OnFixedUpdate() { }
        protected virtual void OnMotionFixedUpdate() { }
        #endregion

        #region Move Velocity
        private void HandleMoveVelocity()
        {
            if (GameManager.TryGetInstance(out var instance) && instance.IsPaused) return;

            CalculateMovementVelocity(
                ControlSource, MoveInput, moveSpeed, ref _playerVelocity, out var result, UseControlRotation);

            OnMoveVelocityHandled(result);
        }

        protected abstract void OnMoveVelocityHandled(MovementResult result);
        #endregion

        #region Pause State Handle
        [EventListener]
        protected void OnPaused(OnPlayerPauseEvent e)
        {
            _cachedVelocity = _playerVelocity;
            _playerVelocity = Vector3.zero;

            OnPlayerPaused(e);
        }

        [EventListener]
        protected void OnUnpaused(OnPlayerUnpauseEvent e)
        {
            if (restoreVelocityAfterPause)
                _playerVelocity = _cachedVelocity;

            _cachedVelocity = Vector3.zero;
            OnPlayerUnpaused(e);
        }

        protected virtual void OnPlayerPaused(OnPlayerPauseEvent e) { }
        protected virtual void OnPlayerUnpaused(OnPlayerUnpauseEvent e) { }
        #endregion
    }
}