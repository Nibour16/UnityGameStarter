using UnityEngine;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Gameplay.Camera.Spatial3D
{
    [System.Serializable]
    public class CameraSettings
    {
        [Header("General")]
        public float distance = 4f;
        public float sensitivity = 1f;

        public float minPitch = -89f;
        public float maxPitch = 89f;

        public float collisionRadius = 0.2f;
        public LayerMask collisionMask = 1 << 0;

        [Header("Advanced")]
        public bool invertVerticalLook = false;
        public bool invertHorizontalLook = false;
    }

    [System.Serializable]
    public struct CameraRuntimeState
    {
        public float pitch;
        public float yaw;

        public Vector3 targetPos;
    }

    public static class CameraControlLibrary
    {
        public static Quaternion CalculateLookRotation(
            Vector2 input, ref float pitch, ref float yaw,
            float sensitivity = 1f, float minPitch = -89f, float maxPitch = 89f, 
            bool invertLookY = false, bool invertLookX = false)
        {
            float yawInput = invertLookX ? -input.x : input.x;
            yaw += yawInput * sensitivity;

            float pitchInput = invertLookY ? input.y : -input.y;
            pitch += pitchInput * sensitivity;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            return Quaternion.Euler(pitch, yaw, 0);
        }

        public static (Vector3, Quaternion) CalculateCameraTransform(
            ref Transform camera, Vector2 input, ref CameraRuntimeState state, CameraSettings setting)
        {
            var resultRot = CalculateLookRotation(
                input, ref state.pitch, ref state.yaw, 
                setting.sensitivity, setting.minPitch, setting.maxPitch,
                setting.invertVerticalLook, setting.invertHorizontalLook);

            var resultPos = TransformLibrary.OrbitPosition(
                state.targetPos, resultRot, setting.distance);

            resultPos = CameraLibrary.ResolveCollision(
                state.targetPos, resultPos, setting.collisionRadius, setting.collisionMask);

            camera.SetPositionAndRotation(resultPos, resultRot);

            return (resultPos, resultRot);
        }

        public static Vector3 GetControlDirection(this Transform source, Vector2 input, bool moveVertically = false) 
        {
            Vector3 direction = source.forward * input.y + source.right * input.x;

            if (!moveVertically) direction.y = 0;

            direction.Normalize();
            return direction;
        }
    }
}