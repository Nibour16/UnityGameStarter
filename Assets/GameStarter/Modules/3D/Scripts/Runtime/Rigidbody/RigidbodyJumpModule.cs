using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Gameplay.PhysicsStatics;

namespace UnityGameStarter.Gameplay.Character.JumpModule.RB3D
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyJumpModule : BaseJumpModule
    {
        [Space]
        [SerializeField] private LayerMask groundMask = 1 << 0;
        [SerializeField] private float maxGroundAngle = 45f;

        private Rigidbody _rb;
        private float _currentGravityScale;

        private readonly Dictionary<Collider, bool> _groundColliders = new();
        private int _validGroundCount;

        public override bool IsGrounded => _validGroundCount > 0;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _currentGravityScale = gravityScale;
        }
        
        private void Update() 
        {
            _currentGravityScale = GetCurrentGravityScale(_rb.linearVelocity.y);
        }
        
        private void FixedUpdate()
        {
            _rb.ApplyExtraGravity(_rb.mass, _currentGravityScale - 1);
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

            if (previous == current) return;

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

        protected override void HandleJump(float jumpStrength)
            => _rb.ApplyForce(jumpStrength, Vector3.up, ForceMode.Impulse);

        private bool IsGround(Collision collision)
            => ((1 << collision.collider.gameObject.layer) & groundMask) != 0;

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