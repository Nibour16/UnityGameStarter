using UnityEngine;
using UnityGameStarter.Math.Vector;

namespace UnityGameStarter.Math.Kinematics 
{
    public static class KinematicsLibrary
    {
        public static float Distance(float speed, float time)
            => speed * time;
        
        public static Vector3 Velocity(Vector3 direction, float speed)
            => VectorLibrary.Scale(direction, speed);

        public static Vector3 VelocityChange(Vector3 acceleration, float time)
            => VectorLibrary.Scale(acceleration, time);

        public static Vector3 Displacement(Vector3 velocity, float time)
            => VectorLibrary.Scale(velocity, time);

        public static Vector3 Displacement(Vector3 velocity, float time, Vector3 acceleration)
        {
            return VectorLibrary.Scale(velocity, time)
                + 0.5f * VectorLibrary.Scale(acceleration, Mathf.Pow(time, 2));
        }

        public static Vector3 Displacement(float speed, Vector3 direction, float time)
            => VectorLibrary.Scale(direction, speed * time);

        public static Vector3 Displacement(float speed, Vector3 direction, float time, Vector3 acceleration)
        {
            return VectorLibrary.Scale(direction, speed * time)
                + 0.5f * VectorLibrary.Scale(acceleration, Mathf.Pow(time, 2));
        }
    }
}

