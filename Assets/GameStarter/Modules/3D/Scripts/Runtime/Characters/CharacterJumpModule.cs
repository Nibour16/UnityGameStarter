using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule 
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterJumpModule : BaseJumpModule
    {
        [SerializeField, Min(0f)] private float gravityValue = 9.81f;

        private CharacterController _motionController;
        private float _velocityY;

        public override bool IsGrounded => _motionController.isGrounded;
        public float VelocityY => _velocityY;

        protected override void Awake()
        {
            base.Awake();
            _motionController = GetComponent<CharacterController>();
        }
        
        protected override void Update()
        {
            base.Update();

            HandleGrounded();
            HandleGravityVelocity();
        }

        public override bool Jump(float overrideJumpStrength = 0)
        {
            if (!_motionController.enabled) return false;

            return base.Jump(overrideJumpStrength);
        }

        public override void ForceJump(float overrideJumpStrength = 0, bool consumeJumpCount = false)
        {
            if (!_motionController.enabled) return;

            base.ForceJump(overrideJumpStrength, consumeJumpCount);
        }

        protected override void HandleJump(float jumpStrength)
        {
            float gravityScale = GetCurrentGravityScale(_velocityY);
            float gravityMagnitude = gravityValue * gravityScale;

            _velocityY = Mathf.Sqrt(2f * jumpStrength * gravityMagnitude);
        }

        private void HandleGrounded()
        {
            if (!_motionController.enabled) return;

            if (IsGrounded && _velocityY < 0)
                _velocityY = 0;
        }

        private void HandleGravityVelocity()
        {
            float gravityMagnitude = 
                -gravityValue * GetCurrentGravityScale(_velocityY) * Time.deltaTime;

            _velocityY += gravityMagnitude;
        }
    }
}