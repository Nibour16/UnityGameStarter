using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Gameplay.PhysicsStatics;

namespace UnityGameStarter.Gameplay.Character.JumpModule 
{
    public abstract class BaseRigidbodyJumpModule<TRigidbody, TCollider> : BaseJumpModule
        where TRigidbody : Component where TCollider : Component
    {
        [Space]
        [SerializeField] protected LayerMask groundMask = 1 << 0;

        /// <summary>
        /// Enable Max Ground Angle used physics cast, which can possibly be more expensive
        /// </summary>
        [SerializeField] private bool enableMaxGroundAngle = false;

        /// <summary>
        /// Only available when EnableMaxGroundAngle boolean is true
        /// </summary>
        [SerializeField] private float maxGroundAngle = 45f;

        protected TRigidbody rb;
        protected float currentGravityScale;

        protected readonly Dictionary<TCollider, bool> groundColliders = new();
        protected int validGroundCount;

        public override bool IsGrounded => validGroundCount > 0;

        protected virtual void Awake()
        {
            rb = GetComponent<TRigidbody>();
            currentGravityScale = gravityScale;
        }

        protected virtual void Update() 
        {
            if (this.rb is Rigidbody rb)
                UpdateGravityScale(rb);
            else if (this.rb is Rigidbody2D rb_2D)
                UpdateGravityScale(rb_2D);
        }

        protected virtual void FixedUpdate()
        {
            if (this.rb is Rigidbody rb)
                rb.ApplyExtraGravity(rb.mass, currentGravityScale - 1);
            else if (this.rb is Rigidbody2D rb_2D)
                rb_2D.ApplyExtraGravity2D(rb_2D.mass, currentGravityScale - 1);
        }

        protected override void HandleJump(float jumpStrength)
        {
            if (this.rb is Rigidbody rb)
                rb.ApplyForce(jumpStrength, Vector3.up, ForceMode.Impulse);
            else if (this.rb is Rigidbody2D rb_2D)
                rb_2D.ApplyForce2D(jumpStrength, Vector2.up, ForceMode2D.Impulse);
        }

        #region API
        public void EnterTrigger(Transform transform, TCollider collider, float maxCheckDistance)
        {
            if (groundColliders.ContainsKey(collider)) return;

            bool isValid = false;

            if (collider is Collider c) 
            {
                if (!IsGround(c)) return;

                isValid = IsValidGround(transform, c, maxCheckDistance);
            }
            else if (collider is Collider2D c2d) 
            {
                if (!IsGround(c2d)) return;

                isValid = IsValidGround(transform, c2d, maxCheckDistance);
            }

            groundColliders[collider] = isValid;

            if (isValid)
                validGroundCount++;
        }

        public void StayTrigger(Transform transform, TCollider collider, float maxCheckDistance)
        {
            if (!groundColliders.TryGetValue(collider, out bool previous))
                return;

            bool current = false;

            if (collider is Collider c)
                current = IsValidGround(transform, c, maxCheckDistance);
            else if (collider is Collider2D c2d)
                current = IsValidGround(transform, c2d, maxCheckDistance);

            if (previous == current) return;

            groundColliders[collider] = current;

            if (current)
                validGroundCount++;
            else
                validGroundCount--;
        }

        public void ExitTrigger(TCollider collider)
        {
            if (!groundColliders.Remove(collider, out bool wasValid))
                return;

            if (wasValid)
                validGroundCount--;
        }
        #endregion

        #region Gravity
        private void UpdateGravityScale(Rigidbody rb)
            => currentGravityScale = GetCurrentGravityScale(rb.linearVelocity.y);

        private void UpdateGravityScale(Rigidbody2D rb)
            => currentGravityScale = GetCurrentGravityScale(rb.linearVelocity.y);
        #endregion

        #region Ground Check
        private bool IsGround(Collider collider)
            => ((1 << collider.gameObject.layer) & groundMask) != 0;

        private bool IsGround(Collider2D collider)
            => ((1 << collider.gameObject.layer) & groundMask) != 0;

        private bool IsValidGround(Transform transform, Collider collider, float maxCheckDistance)
        {
            if (!enableMaxGroundAngle) return true;
            
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.down;

            if (Physics.Raycast(origin, direction, out RaycastHit hit,
                maxCheckDistance, groundMask, QueryTriggerInteraction.Ignore))
                return hit.collider == collider && Vector3.Angle(hit.normal, Vector3.up) <= maxGroundAngle;

            return false;
        }

        private bool IsValidGround(Transform transform, Collider2D collider, float maxCheckDistance)
        {
            if (!enableMaxGroundAngle) return true;

            Vector2 origin = transform.position;
            Vector2 direction = Vector2.down;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxCheckDistance, groundMask);

            return hit.collider == collider && Vector2.Angle(hit.normal, Vector2.up) <= maxGroundAngle;
        }
        #endregion
    }
}