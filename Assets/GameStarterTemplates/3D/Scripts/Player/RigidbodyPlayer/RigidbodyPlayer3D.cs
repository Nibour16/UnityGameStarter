using UnityEngine;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Gameplay.PlayerSystem;
using UnityGameStarter.Gameplay.Character.JumpModule.RB3D;
using UnityGameStarter.Math.TransformStatics;
using static UnityGameStarter.Math.TransformStatics.VectorLibrary;

[RequireComponent(typeof(InputManager3D))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraController3D))]
[RequireComponent(typeof(RigidbodyJumpModule))]
public class RigidbodyPlayer3D : BasePlayer
{
    #region Settings
    [Header("References")]
    [SerializeField] private Transform controlSource;

    [Header("Movement")]
    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;
    [SerializeField] private bool instantTurn = false;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;
    #endregion

    #region References
    private InputManager3D _inputManager;
    private Rigidbody _rb;
    private CameraController3D _cameraController;
    private RigidbodyJumpModule _jumpModule;
    #endregion

    protected override Transform ControlSource => controlSource;
    protected override bool UseControlRotation => useControlRotation;

    private Quaternion _desiredRotation = Quaternion.identity;
    private bool _canRotate = false;

    protected override Vector2 MoveInput => _inputManager.Move;
    protected override bool PauseInput => _inputManager.Pause;

    #region Life Cycle
    protected override void OnInitialized() 
    {
        _inputManager = GetComponent<InputManager3D>();
        _rb = GetComponent<Rigidbody>();
        _cameraController = GetComponent<CameraController3D>();
        _jumpModule = GetComponent<RigidbodyJumpModule>();
    }

    protected override void OnUpdate() 
    {
        HandleJump();
    }

    protected override void OnFixedUpdate() 
    {
        if (_cameraController.enabled) 
        {
            controlSource.rotation = _cameraController.CameraRotation;
            SetPlayerVelocityParam(VectorParam.Y, _rb.linearVelocity.y);
        }
    }

    protected override void OnMotionFixedUpdate()
    {
        Vector3 velocity = _rb.linearVelocity;

        velocity.x = PlayerVelocity.x;
        velocity.z = PlayerVelocity.z;

        _rb.linearVelocity = velocity;

        HandleRotation();
    }

    private void LateUpdate() 
    {
        if (_cameraController.enabled)
            _cameraController.UpdateCamera(controlSource, _inputManager.Look);
    }
    #endregion

    #region Movement
    protected override void OnMoveVelocityHandled(MovementResult result)
    {
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
        if (_jumpModule.isActiveAndEnabled && _inputManager.Jump)
            _jumpModule.Jump();
    }
    #endregion

    #region Pause State Handle
    protected override void OnPlayerPaused(OnPlayerPauseEvent e) 
    {
        _rb.useGravity = false;
        _cameraController.enabled = false;
        _jumpModule.enabled = false;
    }
    protected override void OnPlayerUnpaused(OnPlayerUnpauseEvent e)
    {
        _rb.useGravity = true;
        _cameraController.enabled = true;
        _jumpModule.enabled = true;
    }
    #endregion
}