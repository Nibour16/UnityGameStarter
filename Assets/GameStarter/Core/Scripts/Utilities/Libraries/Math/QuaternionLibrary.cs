using System;
using UnityEngine;

namespace UnityGameStarter.Math.TransformStatics
{
    public enum EulerAxis { X = 0, Y = 1, Z = 2}
    
    public class QuaternionBasis
    {
        public Vector3 forward = Vector3.forward;
        public Vector3 right = Vector3.right;
        public Vector3 up = Vector3.up;
    }

    public static class QuaternionLibrary
    {
        #region Euler Axis
        public static Quaternion SetEulerComponent(this Quaternion rotation, EulerAxis axis, float value)
        {
            Vector3 euler = rotation.eulerAngles;
            euler[(int)axis] = value;

            return Quaternion.Euler(euler);
        }

        public static Quaternion SetEulerComponent(this Quaternion targetRot, EulerAxis axis, Quaternion desiredRot)
        {
            var value = axis switch
            {
                EulerAxis.X => desiredRot.GetPitch(),
                EulerAxis.Y => desiredRot.GetYaw(),
                EulerAxis.Z => desiredRot.GetRoll(),
                _ => throw new ArgumentOutOfRangeException(nameof(axis))
            };

            return targetRot.SetEulerComponent(axis, value);
        }

        public static Quaternion SetEulerComponent(this Quaternion rotation, EulerAxis axis, Transform trans)
            => rotation.SetEulerComponent(axis, trans.rotation);

        public static void SetEulerComponent(ref Quaternion rotation, EulerAxis axis, float value)
            => rotation = rotation.SetEulerComponent(axis, value);

        public static void SetEulerComponent(ref Quaternion targetRot, EulerAxis axis, Quaternion desiredRot)
            => targetRot = targetRot.SetEulerComponent(axis, desiredRot);

        public static void SetEulerComponent(ref Quaternion rotation, EulerAxis axis, Transform trans)
            => rotation = rotation.SetEulerComponent(axis, trans);
        #endregion

        #region Quaternion Basis
        public static QuaternionBasis GetAxis(this Quaternion rotation)
        {
            return new QuaternionBasis
            {
                forward = rotation * Vector3.forward,
                right = rotation * Vector3.right,
                up = rotation * Vector3.up
            };
        }

        public static Vector3 Forward(this Quaternion rotation) => rotation.GetAxis().forward;

        public static Vector3 Right(this Quaternion rotation) => rotation.GetAxis().right;

        public static Vector3 Up(this Quaternion rotation) => rotation.GetAxis().up;

        public static Vector3 FlatForward(this Quaternion rotation)
        {
            Vector3 forward = rotation.Forward();
            forward.y = 0;

            return forward.normalized;
        }

        public static Vector3 FlatRight(this Quaternion rotation)
        {
            Vector3 right = rotation.Right();
            right.y = 0;

            return right.normalized;
        }
        #endregion

        #region Quaternion Angle
        public static float GetPitch(this Quaternion rotation)
        {
            Vector3 euler = rotation.eulerAngles;
            return euler.x;
        }

        public static float GetYaw(this Quaternion rotation)
        {
            Vector3 euler = rotation.eulerAngles;
            return euler.y;
        }

        public static float GetRoll(this Quaternion rotation)
        {
            Vector3 euler = rotation.eulerAngles;
            return euler.z;
        }

        public static float NormalizeAngle(this float angle)
        {
            angle %= 360f;

            if (angle > 180f) angle -= 360f;

            if (angle < -180f) angle += 360f;

            return angle;
        }

        public static float GetAngleDifference(float current, float target)
            => Mathf.DeltaAngle(current, target);
        #endregion

        #region Quanterion Look Rotation
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

        public static bool ToLook(
            this Vector3 position, Vector3 target, out Quaternion lookRotation, float sqrThreshold = 0.001f)
            => ToLook(target - position, out lookRotation, sqrThreshold);

        public static bool ToLook(
            this Vector2 position, Vector2 target, out Quaternion lookRotation, float sqrThreshold = 0.001f)
            => ToLook(target - position, out lookRotation, sqrThreshold);

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
        #endregion
    }
}