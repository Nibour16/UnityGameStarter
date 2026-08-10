using UnityEngine;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Gameplay.PlayerSystem;

[RequireComponent(typeof(InputManager3D))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CameraController3D))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(EventListenerRegister))]
public class Player3D : MonoBehaviour, IAutoEventListener
{
    #region Serialized Fields
    [Header("Reference")]
    [SerializeField] private Transform controlSource;

    [Header("Move")]
    [SerializeField, Min(0.01f)] private float moveSpeed = 4f;

    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;
    [SerializeField] private bool instantTurn = false;

    [Header("Jump")]
    [SerializeField, Min(0.1f)] private float jumpHeight = 1.5f;
    [SerializeField, Min(0f)] private float gravityValue = 9.81f;
    [SerializeField, Min(0f)] private float jumpGravityScale = 1f;
    [SerializeField, Min(0f)] private float fallingGravityScale = 1f;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;

    [Header("Advanced")]
    [SerializeField] private bool resetVelocityWhenPaused = false;
    #endregion

    #region References
    private InputManager3D _inputManager;

    private CharacterController _motionController;
    private CameraController3D _cameraController;
    private PlayerController _playerController;
    #endregion

    #region Private Fields
    private Vector3 _playerVelocity = Vector3.zero;
    private Vector3 _cachedVelocity = Vector3.zero;
    private Quaternion _desiredRotation;
    private bool _canRotate = false;
    private bool _isGrounded = false;
    #endregion

    #region Life Cycle
    private void Awake()
    {
        _inputManager = GetComponent<InputManager3D>();

        _motionController = GetComponent<CharacterController>();
        _cameraController = GetComponent<CameraController3D>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update() 
    {
        HandleControlSource();
        MakeMove();

        if (_inputManager.Pause)
            _playerController.SetPause();
    }

    private void LateUpdate() 
    {
        HandleCamera();
    }
    #endregion

    #region Camera Control
    private void HandleControlSource()
    {
        if (_cameraController.enabled)
            controlSource.rotation = _cameraController.CameraRotation;
    }

    private void HandleCamera() 
    {
        if (_cameraController.enabled)
            _cameraController.UpdateCamera(controlSource, _inputManager.Look);
    }
    #endregion

    #region Movement
    private void MakeMove() 
    {
        if (!_motionController.enabled) return;
        
        HandleGrounded();
        HandleMoveVelocity();
        HandleJumpVelocity();
        HandleGravityVelocity();
        HandleRotation();

        _motionController.Move(_playerVelocity * Time.deltaTime);
    }

    // Horizontal
    private void HandleMoveVelocity() 
    {
        MovementLibrary.CalculateMovementVelocity(
            controlSource, _inputManager.Move, moveSpeed, ref _playerVelocity, out var result, useControlRotation);

        _desiredRotation = result.rotation;
        _canRotate = result.hasRotation;
    }

    // Vertical
    private void HandleGrounded() 
    {
        _isGrounded = _motionController.isGrounded;

        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;
    }

    private void HandleJumpVelocity() 
    {
        if (_isGrounded && _inputManager.Jump)
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * gravityValue * (jumpGravityScale + 1));
    }

    private void HandleGravityVelocity() 
    {
        _playerVelocity.y -= gravityValue * fallingGravityScale * Time.deltaTime;
    }

    // Rotation
    private void HandleRotation()
    {
        if (!_canRotate) return;

        if (instantTurn)
        {
            transform.rotation = _desiredRotation;
            return;
        }

        Quaternion rotation = transform.rotation.GetTurnRotation(
            _desiredRotation, turnSpeed, Time.deltaTime);

        transform.rotation = rotation;
    }
    #endregion

    #region Pause State Handle
    [EventListener]
    private void OnPaused(OnPlayerPauseEvent e)
    {
        _cachedVelocity = _playerVelocity;
        _playerVelocity = Vector3.zero;

        _motionController.enabled = false;
        _cameraController.enabled = false;
    }

    [EventListener]
    private void OnUnpaused(OnPlayerUnpauseEvent e)
    {
        if (!resetVelocityWhenPaused)
            _playerVelocity = _cachedVelocity;

        _motionController.enabled = true;
        _cameraController.enabled = true;

        _cachedVelocity = Vector3.zero;
    }
    #endregion
}