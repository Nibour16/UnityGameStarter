using UnityEngine;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.InputSystem.Player.Player_3D;

namespace UnityGameStarter.Gameplay.Player3D.Sample
{
    [RequireComponent(typeof(Rigidbody))]
    public class SamplePlayer3D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Min(0.01f)] private float moveSpeed = 2f;
        //[SerializeField, Min(0.01f)] private float jumpIntensity = 1f;
        [SerializeField, Min(0.01f)] private float turnSpeed = 10f;

        [Header("Player Setting")]
        [SerializeField] private bool useControlRotation = true;

        private Rigidbody _rb;

        private void Awake() 
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() 
        {
            Move();
        }

        private void Move() 
        {
            Vector2 direction = InputFacade3D.Instance.Movement;

            MovementLibrary.CalculateMovement(direction, moveSpeed, out var result, useControlRotation);
            _rb.linearVelocity = result.velocity;

            if (result.hasRotation) 
            {
                Quaternion rotation = _rb.rotation.GetTurnRotation(
                    result.rotation, turnSpeed, Time.fixedDeltaTime);

                _rb.MoveRotation(rotation); 
            }
        }
    }
}