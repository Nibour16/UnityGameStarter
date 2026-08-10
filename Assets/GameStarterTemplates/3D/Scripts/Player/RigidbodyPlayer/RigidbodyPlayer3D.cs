using UnityEngine;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Gameplay;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Gameplay.PlayerSystem;
using UnityGameStarter.Gameplay.Rigidbody3DExtension;

[RequireComponent(typeof(InputManager3D))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraController3D))]
[RequireComponent(typeof(RigidbodyJumpModule))]
[RequireComponent(typeof(EventListenerRegister))]
[RequireComponent(typeof(PlayerController))]
public class RigidbodyPlayer3D : MonoBehaviour, IAutoEventListener
{
    #region Settings
    [Header("References")]
    [SerializeField] private Transform controlSource;

    [Header("Movement")]
    [SerializeField, Min(0.01f)] private float moveSpeed = 4f;
    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;
    [SerializeField] private bool instantTurn = false;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;

    [Header("Advanced")]
    [SerializeField] private bool resetVelocityWhenPaused = false;
    #endregion

    #region References
    private InputManager3D _inputManager;
    private Rigidbody _rb;
    private CameraController3D _cameraController;
    private RigidbodyJumpModule _jumpModule;
    private PlayerController _playerController;
    #endregion

    private Quaternion _desiredRotation = Quaternion.identity;
    private bool _canRotate = false;
    private Vector3 _cachedVelocity = Vector3.zero;

    #region Life Cycle
    private void Awake() 
    {
        _inputManager = GetComponent<InputManager3D>();

        _rb = GetComponent<Rigidbody>();

        _cameraController = GetComponent<CameraController3D>();
        _jumpModule = GetComponent<RigidbodyJumpModule>();

        _playerController = GetComponent<PlayerController>();
    }

    private void Update() 
    {
        HandleJump();
        
        if (_inputManager.Pause)
            _playerController.SetPause();
    }

    private void FixedUpdate() 
    {
        if (_cameraController.enabled)
            controlSource.rotation = _cameraController.CameraRotation;

        HandleMove();
        HandleRotation();

        if (_jumpModule.isActiveAndEnabled)
            _jumpModule.ResolveGravity();
    }

    private void LateUpdate() 
    {
        if (_cameraController.enabled)
            _cameraController.UpdateCamera(controlSource, _inputManager.Look);
    }
    #endregion

    #region Movement
    private void HandleMove()
    {
        if (GameManager.TryGetInstance(out var instance) && instance.IsPaused) return;
        
        Vector3 velocity = _rb.linearVelocity;

        MovementLibrary.CalculateMovementVelocity(
            controlSource, _inputManager.Move, moveSpeed, ref velocity, out var result, useControlRotation);

        _rb.linearVelocity = velocity;
        _desiredRotation = result.rotation;
        _canRotate = result.hasRotation;
    }

    private void HandleRotation() 
    {
        if (!_canRotate) return;

        if (instantTurn) 
        {
            _rb.rotation = _desiredRotation;
            return;
        }

        Quaternion rotation = _rb.rotation.GetTurnRotation(
            _desiredRotation, turnSpeed, Time.fixedDeltaTime);

        _rb.rotation = rotation;
    }

    private void HandleJump() 
    {
        if (_jumpModule.isActiveAndEnabled)
        {
            if (_inputManager.Jump)
                _jumpModule.Jump();

            _jumpModule.UpdateGravityState();
        }
    }
    #endregion

    #region Pause State Handle
    [EventListener]
    private void OnPaused(OnPlayerPauseEvent e)
    {
        _cachedVelocity = _rb.linearVelocity;
        
        _rb.useGravity = false;
        _cameraController.enabled = false;
        _jumpModule.enabled = false;
    }

    [EventListener]
    private void OnUnpaused(OnPlayerUnpauseEvent e)
    {
        if (!resetVelocityWhenPaused)
            _rb.linearVelocity = _cachedVelocity;

        _rb.useGravity = true;
        _cameraController.enabled = true;
        _jumpModule.enabled = true;

        _cachedVelocity = Vector3.zero;
    }
    #endregion
}