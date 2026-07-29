using UnityEngine;

namespace UnityGameStarter.Math.TransformStatics 
{
    public static class TransformLibrary
    {
        public static Vector3 OrbitPosition(Vector3 target, float yaw, float pitch, float distance)
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            return OrbitPosition(target, rotation, distance);
        }

        public static Vector3 OrbitPosition(Vector3 target, Quaternion rotation, float distance) 
        {
            Vector3 offset = rotation * new Vector3(0, 0, -distance);
            return target + offset;
        }

        public static Vector3 SmoothFollow(Vector3 current, Vector3 target, ref Vector3 velocity,
            float smoothTime, float deltaTime)
            => Vector3.SmoothDamp(current, target, ref velocity, smoothTime, Mathf.Infinity, deltaTime);


        public static float GetPitch(this Transform trans)
            => trans.rotation.GetPitch();

        public static float GetYaw(this Transform trans)
            => trans.rotation.GetYaw();

        public static float GetRoll(this Transform trans)
            => trans.rotation.GetRoll();
    }
}