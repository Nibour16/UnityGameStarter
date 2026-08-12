using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule 
{
    public abstract class BaseJumpModule : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] protected float jumpStrength = 6f;
        [SerializeField, Min(0f)] protected float gravityScale = 1f;
        [SerializeField, Min(0f)] protected float fallingGravityScale = 1f;

        public abstract bool IsGrounded { get; }

        public void Jump() 
        {
            if (!IsGrounded) return;

            HandleJump();
        }

        protected float GetCurrentGravityScale(float velocityY)
            => velocityY >= 0 ? gravityScale : fallingGravityScale;

        protected abstract void HandleJump();
    }
}