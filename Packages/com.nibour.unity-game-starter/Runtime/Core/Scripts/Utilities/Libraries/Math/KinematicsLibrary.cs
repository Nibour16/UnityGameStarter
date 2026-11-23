using UnityEngine;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Math.Kinematics 
{
    public static class KinematicsLibrary
    {
        public static float Distance(float speed, float time) => speed * time;
        
        public static Vector3 Velocity(Vector3 direction, float speed) => direction.normalized * speed;

        public static Vector3 Velocity(Vector2 direction, float speed)
        {
            var resolvedDirection = direction.ToVector3XZ();
            return Velocity(resolvedDirection, speed);
        }

        public static Vector2 Velocity2D(Vector2 direction, float speed) => direction.normalized * speed;

        public static Vector3 VelocityChange(Vector3 acceleration, float time) => acceleration * time;

        public static Vector2 VelocityChange2D(Vector2 acceleration, float time) => acceleration * time;

        public static Vector3 Displacement(Vector3 velocity, float time) => velocity * time;

        public static Vector3 Displacement(Vector3 velocity, float time, Vector3 acceleration)
            => velocity * time + 0.5f * Mathf.Pow(time, 2) * acceleration;

        public static Vector3 Displacement(float speed, Vector3 direction, float time)
            => speed * time * direction;

        public static Vector3 Displacement(float speed, Vector3 direction, float time, Vector3 acceleration)
            => speed * time * direction + 0.5f * Mathf.Pow(time, 2) * acceleration;

        public static Vector2 Displacement2D(Vector2 velocity, float time) => velocity * time;

        public static Vector2 Displacement2D(Vector2 velocity, float time, Vector2 acceleration)
            => Displacement(velocity.ToVector3XZ(), time, acceleration.ToVector3XZ());

        public static Vector2 Displacement2D(float speed, Vector2 direction, float time) 
            => speed * time * direction;

        public static Vector2 Displacement2D(float speed, Vector2 direction, float time, Vector2 acceleration)
            => speed * time * direction + 0.5f * Mathf.Pow(time, 2) * acceleration;
    }
}