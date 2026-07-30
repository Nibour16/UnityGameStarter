using UnityEngine;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.InputSystem.Player.Player_3D;

namespace UnityGameStarter.Gameplay.Player3D.Sample
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CameraController))]
    public class SamplePlayer3D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Min(0.01f)] private float moveSpeed = 2f;
        //[SerializeField, Min(0.01f)] private float jumpIntensity = 1f;
        [SerializeField, Min(0.01f)] private float turnSpeed = 10f;

        [Header("Player Setting")]
        [SerializeField] private bool useControlRotation = true;

        private Rigidbody _rb;
        private CameraController _cameraController;

        private void Awake() 
        {
            _rb = GetComponent<Rigidbody>();
            _cameraController = GetComponent<CameraController>();
        }

        private void FixedUpdate() 
        {
            Move();
        }

        private void Move() 
        {
            Vector2 input = InputFacade3D.Instance.Movement;
            Transform source = _cameraController.TargetSource;

            MovementLibrary.CalculateMovement(source, input, moveSpeed, out var result, useControlRotation);
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