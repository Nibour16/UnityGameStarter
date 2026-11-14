using UnityEngine;
using UnityGameStarter.TimerSystem;

namespace UnityGameStarter.Gameplay.Character.JumpModule 
{
    public abstract class BaseJumpModule : MonoBehaviour
    {
        [Header("General")]
        [SerializeField, Min(0.1f)] private float jumpStrength = 6f;
        [SerializeField, Min(0f)] protected float gravityScale = 1f;
        [SerializeField, Min(0f)] protected float fallingGravityScale = 1f;

        [Header("Coyote Time!")]
        [SerializeField, Min(0f)] private float coyoteTime = 0;

        [Header("Jump Counter")]
        /// <summary>
        /// Total jump count, including the first grounded jump.
        /// Only used in MultiJump mode.
        /// </summary>
        [SerializeField, Min(0)] private int extraJumpCount = 0;

        [Header("Flappy Jumper")]
        public bool flappyJump = false;

        private int _remainingExtraJumpCount;
        private bool _wasGrounded;
        private Timer _coyoteTimer;

        public abstract bool IsGrounded { get; }
        public virtual bool CanAirJump => _remainingExtraJumpCount > 0;
        public virtual bool CanJump => flappyJump || IsGrounded || IsCoyoteTimeActive || CanAirJump;

        public int RemainingExtraJumpCount => _remainingExtraJumpCount;

        protected bool IsCoyoteTimeActive => coyoteTime > 0f && _coyoteTimer != null
            && _coyoteTimer.TimerState == TimerState.Running;

        protected virtual void Awake() 
        {
            RefreshJumpCount();
        }

        protected virtual void Update()
        {
            bool isGrounded = IsGrounded;

            if (isGrounded && !_wasGrounded)
                StopCoyoteTimer();
            else if (!isGrounded && _wasGrounded)
                StartCoyoteTimer();

            if (isGrounded)
                RefreshJumpCount();

            _wasGrounded = isGrounded;
        }

        protected void RefreshJumpCount()
        {
            _remainingExtraJumpCount = extraJumpCount;
        }

        private void StartCoyoteTimer() 
        {
            if (coyoteTime <= 0f)
                return;

            StopCoyoteTimer();

            _coyoteTimer = TimerManager.Instance.CreateTimer(
                this, coyoteTime, "Player Coyote Timer", TimerType.AutoRemovable);
        }

        private void StopCoyoteTimer() 
        {
            if (_coyoteTimer == null)
                return;

            TimerManager.Instance.Unregister(_coyoteTimer);
            _coyoteTimer = null;
        }

        public virtual bool Jump(float overrideJumpStrength = 0) 
        {
            if (!CanJump) return false;

            float jumpStrength = overrideJumpStrength <= 0 ? this.jumpStrength : overrideJumpStrength;

            if (!IsGrounded && !IsCoyoteTimeActive && !flappyJump)
                _remainingExtraJumpCount--;

            HandleJump(jumpStrength);
            return true;
        }

        public virtual void ForceJump(float overrideJumpStrength = 0, bool consumeJumpCount = false) 
        {
            float jumpStrength = overrideJumpStrength <= 0 ? this.jumpStrength : overrideJumpStrength;

            if (consumeJumpCount && !IsGrounded && !IsCoyoteTimeActive && !flappyJump)
                _remainingExtraJumpCount = Mathf.Max(0, _remainingExtraJumpCount - 1);

            HandleJump(jumpStrength);
        }

        protected float GetCurrentGravityScale(float velocityY)
            => velocityY >= 0 ? gravityScale : fallingGravityScale;

        protected abstract void HandleJump(float jumpStrength);
    }
}