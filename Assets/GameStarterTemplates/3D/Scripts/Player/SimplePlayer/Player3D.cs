using UnityEngine;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Gameplay.Character.JumpModule;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Gameplay.PlayerSystem;
using UnityGameStarter.Math.TransformStatics;

[RequireComponent(typeof(InputManager3D))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CameraController3D))]
[RequireComponent(typeof(CharacterJumpModule))]
public class Player3D : BasePlayer
{
    #region Serialized Fields
    [Header("Reference")]
    [SerializeField] private Transform controlSource;
    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;
    [SerializeField] private bool instantTurn = false;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;
    #endregion

    #region References
    private InputManager3D _inputManager;
    private CharacterController _motionController;
    private CameraController3D _cameraController;
    private CharacterJumpModule _jumpModule;
    #endregion

    #region Private Fields
    private Quaternion _desiredRotation;
    private bool _canRotate = false;
    #endregion

    #region Properties Override
    protected override Transform ControlSource => controlSource;
    protected override bool UseControlRotation => useControlRotation;
    protected override bool EnableMotion => _motionController.enabled;

    protected override Vector2 MoveInput => _inputManager.Move;
    protected override bool PauseInput => _inputManager.Pause;
    #endregion

    #region Life Cycle
    private void LateUpdate() 
    {
        HandleCamera();
    }

    protected override void OnInitialized()
    {
        _inputManager = GetComponent<InputManager3D>();
        _motionController = GetComponent<CharacterController>();
        _cameraController = GetComponent<CameraController3D>();
        _jumpModule = GetComponent<CharacterJumpModule>();
    }

    protected override void OnUpdate()
    {
        HandleControlSource();
    }

    protected override void OnMotionUpdate()
    {
        HandleJump();
        SetPlayerVelocityParam(VectorParam.Y, _jumpModule.VelocityY);
        HandleRotation();

        _motionController.Move(PlayerVelocity * Time.deltaTime);
    }

    protected override void OnMoveVelocityHandled(MovementResult result)
    {
        _desiredRotation = result.rotation;
        _canRotate = result.hasRotation;
    }

    protected override void OnPlayerPaused(OnPlayerPauseEvent e)
    {
        _motionController.enabled = false;
        _cameraController.enabled = false;
    }

    protected override void OnPlayerUnpaused(OnPlayerUnpauseEvent e)
    {
        _motionController.enabled = true;
        _cameraController.enabled = true;
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

    #region Move Rotation
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

    #region Jump
    private void HandleJump()
    {
        if (_jumpModule.isActiveAndEnabled && _inputManager.Jump)
            _jumpModule.Jump();
    }
    #endregion
}