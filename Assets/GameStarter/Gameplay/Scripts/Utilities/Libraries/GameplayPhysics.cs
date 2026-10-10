using UnityEngine;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Gameplay.PhysicsStatics 
{
    public static class GameplayPhysics
    {
        public static void ApplyForce(
            this Rigidbody rb, float strength, Vector3 direction, ForceMode forceMode = ForceMode.Force)
            => rb.AddForce(strength * direction, forceMode);

        public static void ApplyForce2D(
            this Rigidbody2D rb, float strength, Vector3 direction, ForceMode2D forceMode = ForceMode2D.Force)
            => rb.AddForce(strength * direction, forceMode);

        public static void ApplyExtraGravity(
            this Rigidbody rb, float mass, float gravityScale, ForceMode forceMode = ForceMode.Force)
            => rb.ApplyForce(mass, Physics.gravity.Scale(gravityScale), forceMode);

        public static void ApplyExtraGravity2D(
            this Rigidbody2D rb, float mass, float gravityScale, ForceMode2D forceMode = ForceMode2D.Force)
            => rb.ApplyForce2D(mass, Physics.gravity.Scale(gravityScale), forceMode);
    }
}