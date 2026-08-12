using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule.RB2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DJumpModule : BaseJumpModule
    {
        [Space]
        [SerializeField] private LayerMask groundMask = 1 << 0;
        [SerializeField] private float maxGroundAngle = 45f;

        private Rigidbody2D _rb;
        private readonly Dictionary<Collider2D, bool> _groundColliders = new();
        private int _validGroundCount;

        public override bool IsGrounded => _validGroundCount > 0;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.gravityScale = GetCurrentGravityScale(_rb.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsGround(collision)) return;

            bool isValid = IsValidGround(collision);

            _groundColliders[collision.collider] = isValid;

            if (isValid)
                _validGroundCount++;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!_groundColliders.TryGetValue(collision.collider, out bool previous))
                return;

            bool current = IsValidGround(collision);

            if (previous == current) return;

            _groundColliders[collision.collider] = current;

            if (current)
                _validGroundCount++;
            else
                _validGroundCount--;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!_groundColliders.Remove(collision.collider, out bool wasValid))
                return;

            if (wasValid)
                _validGroundCount--;
        }

        protected override void HandleJump()
            => _rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

        private bool IsGround(Collision2D collision)
            => ((1 << collision.collider.gameObject.layer) & groundMask) != 0;

        private bool IsValidGround(Collision2D collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (Vector2.Angle(contact.normal, Vector2.up) <= maxGroundAngle)
                    return true;
            }

            return false;
        }
    }
}