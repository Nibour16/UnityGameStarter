using UnityEngine;
using UnityEngine.Windows;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Math.Kinematics;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Gameplay.CharacterMovement 
{
    public static class MovementLibrary
    {
        public struct MovementResult 
        {
            public Vector3 velocity;
            public Quaternion rotation;
            public bool hasRotation;
        }
        
        public static void CalculateMovement(
            Vector2 direction, float moveSpeed, out MovementResult movement, bool updateTurn = true) 
        {
            movement.velocity = KinematicsLibrary.Velocity(direction, moveSpeed);

            if (updateTurn && direction.ToLook(out var result)) 
            {
                movement.rotation = result;
                movement.hasRotation = true;
            }
            else 
            {
                movement.rotation = Quaternion.identity;
                movement.hasRotation = false;
            }
        }

        public static void CalculateMovement(
            Transform source, Vector2 input, float moveSpeed, out MovementResult movement, bool updateTurn = true)
        {
            Vector2 direction = source.GetControlDirection(input).XZToVector2();
            CalculateMovement(direction, moveSpeed, out movement, updateTurn);
        }

        public static void CalculateMovementVelocity(
            Transform source, Vector2 input, float moveSpeed, ref Vector3 targetVelocity, 
            out MovementResult movement, bool updateTurn = true)
        {
            CalculateMovement(source, input, moveSpeed, out movement, updateTurn);
            targetVelocity = new Vector3(movement.velocity.x, targetVelocity.y, movement.velocity.z);
        }

        public static Quaternion GetTurnRotation(this Quaternion current, Quaternion target,
            float turnSpeed, float deltaTime)
            => current.TurnTowards(target, turnSpeed, deltaTime);
    }
}