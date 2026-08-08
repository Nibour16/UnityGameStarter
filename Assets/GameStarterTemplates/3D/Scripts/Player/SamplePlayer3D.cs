using UnityEngine;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Math.TransformStatics;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SamplePlayerCameraController))]
public class SamplePlayer3D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0.01f)] private float moveSpeed = 2f;
    //[SerializeField, Min(0.01f)] private float jumpIntensity = 1f;
    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;

    private Rigidbody _rb;
    private SamplePlayerCameraController _cameraController;

    private void Awake() 
    {
        _rb = GetComponent<Rigidbody>();
        _cameraController = GetComponent<SamplePlayerCameraController>();
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private void Move() 
    {
        Vector2 input = InputFacade3D.Instance.Movement;
        Transform source = _cameraController.TargetSource;

        Vector3 velocity = _rb.linearVelocity;

        MovementLibrary.CalculateMovementVelocity(
            source, input, moveSpeed, ref velocity, out var result, useControlRotation);

        _rb.linearVelocity = velocity;

        if (result.hasRotation) 
        {
            Quaternion rotation = _rb.rotation.GetTurnRotation(
                result.rotation, turnSpeed, Time.fixedDeltaTime);

            _rb.MoveRotation(rotation); 
        }
    }
}