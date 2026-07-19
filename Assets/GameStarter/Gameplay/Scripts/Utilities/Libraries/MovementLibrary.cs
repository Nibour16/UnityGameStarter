using UnityEngine;
using UnityGameStarter.Math.Kinematics;
using UnityGameStarter.Math.QuaternionStrategy;

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

        public static Quaternion GetTurnRotation(this Quaternion current, Quaternion target,
            float turnSpeed, float deltaTime)
            => current.TurnTowards(target, turnSpeed, deltaTime);
    }
}