using UnityEngine;
using UnityGameStarter.Math.Vector;

namespace UnityGameStarter.Math.QuaternionStrategy
{
    public static class QuaternionLibrary
    {
        public static bool ToLook(this Vector3 direction, out Quaternion lookRotation, float sqrThreshold = 0.001f) 
        {
            if (direction.sqrMagnitude > sqrThreshold)
            {
                lookRotation = Quaternion.LookRotation(direction);
                return true;
            }

            lookRotation = Quaternion.identity;
            return false;
        }

        public static bool ToLook(this Vector2 direction, out Quaternion lookRotation, float sqrThreshold = 0.001f) 
        {
            var resolvedDirection = VectorLibrary.ToVector3XZ(direction);
            return ToLook(resolvedDirection, out lookRotation, sqrThreshold);
        }

        public static Quaternion TurnTowards(this Quaternion current, Quaternion target,
            float turnSpeed, float deltaTime)
        {
            float angle = Quaternion.Angle(current, target);
            float delta = angle * turnSpeed * deltaTime;
            return Quaternion.RotateTowards(current, target, delta);
        }

        public static Quaternion SmoothTowards(this Quaternion current, Quaternion target,
            float sharpness, float deltaTime, float threshold = 0.1f)
        {
            if (current.IsRotatedToTarget(target, threshold)) return target;

            float t = 1f - Mathf.Exp(-sharpness * deltaTime);
            return Quaternion.Slerp(current, target, t);
        }

        public static Quaternion RotateTo(this Quaternion current, Quaternion target,
            float duration, float deltaTime, float threshold = 0.1f)
        {
            if (current.IsRotatedToTarget(target, threshold)) return target;

            float t = deltaTime / duration;
            return Quaternion.Slerp(current, target, t);
        }

        public static bool IsRotatedToTarget(this Quaternion current, Quaternion target, float threshold = 0.1f) 
        {
            float angle = Quaternion.Angle(current, target);
            return angle <= threshold;
        }
    }
}