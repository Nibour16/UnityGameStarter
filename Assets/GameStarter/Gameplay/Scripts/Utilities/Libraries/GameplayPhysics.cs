using UnityEngine;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Gameplay.PhysicsStatics 
{
    public static class GameplayPhysics
    {
        public static void ApplyForce(
            this Rigidbody rb, float strength, Vector3 direction, ForceMode forceMode = ForceMode.Force)
            => rb.AddForce(strength * direction, forceMode);

        public static void ApplyExtraGravity(
            this Rigidbody rb, float mass, float gravityScale)
            => rb.ApplyForce(mass, Physics.gravity.Scale(gravityScale));
    }
}