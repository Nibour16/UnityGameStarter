using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule 
{
    public abstract class BaseJumpModule : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float jumpStrength = 6f;
        [SerializeField, Min(0f)] protected float gravityScale = 1f;
        [SerializeField, Min(0f)] protected float fallingGravityScale = 1f;

        public bool flappyJump = false;
        public abstract bool IsGrounded { get; }

        public void Jump(float overrideJumpStrength = 0) 
        {
            if (!(IsGrounded || flappyJump)) return;

            float jumpStrength = overrideJumpStrength <= 0 ? this.jumpStrength : overrideJumpStrength;
            HandleJump(jumpStrength);
        }

        public void ForceJump(float overrideJumpStrength = 0) 
        {
            float jumpStrength = overrideJumpStrength <= 0 ? this.jumpStrength : overrideJumpStrength;
            HandleJump(jumpStrength);
        }

        protected float GetCurrentGravityScale(float velocityY)
            => velocityY >= 0 ? gravityScale : fallingGravityScale;

        protected abstract void HandleJump(float jumpStrength);
    }
}