using UnityEngine;
using UnityGameStarter.Math.Transform.Vector;

namespace UnityGameStarter.Math.Kinematics 
{
    public static class KinematicsLibrary
    {
        public static float Distance(float speed, float time)
            => speed * time;
        
        public static Vector3 Velocity(Vector3 direction, float speed)
            => VectorLibrary.Scale(direction.normalized, speed);

        public static Vector3 Velocity(Vector2 direction, float speed)
        {
            var resolvedDirection = direction.ToVector3XZ();
            return Velocity(resolvedDirection, speed);
        }

        public static Vector2 Velocity2D(Vector2 direction, float speed)
            => VectorLibrary.Scale(direction.normalized, speed);

        public static Vector3 VelocityChange(Vector3 acceleration, float time)
            => VectorLibrary.Scale(acceleration, time);

        public static Vector2 VelocityChange2D(Vector2 acceleration, float time)
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

        public static Vector2 Displacement2D(Vector2 velocity, float time)
            => VectorLibrary.Scale(velocity, time);

        public static Vector2 Displacement2D(Vector2 velocity, float time, Vector2 acceleration)
        {
            return Displacement(velocity.ToVector3XZ(), time, acceleration.ToVector3XZ());
        }

        public static Vector2 Displacement2D(float speed, Vector2 direction, float time)
            => VectorLibrary.Scale(direction, speed * time);

        public static Vector2 Displacement2D(float speed, Vector2 direction, float time, Vector2 acceleration)
        {
            return VectorLibrary.Scale(direction, speed * time)
                + 0.5f * VectorLibrary.Scale(acceleration, Mathf.Pow(time, 2));
        }
    }
}