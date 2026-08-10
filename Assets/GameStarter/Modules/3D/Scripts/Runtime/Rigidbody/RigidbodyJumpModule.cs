using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Gameplay.PhysicsStatics;

namespace UnityGameStarter.Gameplay.Rigidbody3DExtension 
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyJumpModule : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float jumpStrength = 6f;
        [SerializeField, Min(0f)] private float gravityScale = 1f;
        [SerializeField, Min(0f)] private float fallingGravityScale = 1f;
        [Space]
        [SerializeField] private LayerMask groundMask = 1 << 0;
        [SerializeField] private float maxGroundAngle = 45f;

        private Rigidbody _rb;
        private float _currentGravityScale;

        private readonly Dictionary<Collider, bool> _groundColliders = new();
        private int _validGroundCount;

        public bool IsGrounded => _validGroundCount > 0;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _currentGravityScale = gravityScale;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsGround(collision)) return;

            bool isValid = IsValidGround(collision);

            _groundColliders[collision.collider] = isValid;

            if (isValid)
                _validGroundCount++;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!_groundColliders.TryGetValue(collision.collider, out bool previous))
                return;

            bool current = IsValidGround(collision);

            if (previous == current)
                return;

            _groundColliders[collision.collider] = current;

            if (current)
                _validGroundCount++;
            else
                _validGroundCount--;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!_groundColliders.Remove(collision.collider, out bool wasValid))
                return;

            if (wasValid)
                _validGroundCount--;
        }

        public void Jump()
        {
            if (!IsGrounded) return;

            _rb.ApplyForce(jumpStrength, Vector3.up, ForceMode.Impulse);
        }

        public void ResolveGravity()
        {
            _rb.ApplyExtraGravity(_rb.mass, _currentGravityScale - 1);
        }

        public void UpdateGravityState()
        {
            if (_rb.linearVelocity.y >= 0)
                _currentGravityScale = gravityScale;
            else
                _currentGravityScale = fallingGravityScale;
        }

        private bool IsGround(Collision collision)
            => (groundMask.value & (1 << collision.gameObject.layer)) != 0;

        private bool IsValidGround(Collision collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (Vector3.Angle(contact.normal, Vector3.up) <= maxGroundAngle)
                    return true;
            }

            return false;
        }
    }
}