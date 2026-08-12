using UnityEngine;
using UnityGameStarter.Gameplay.Camera.Spatial3D;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Gameplay.PlayerSystem;
using static UnityGameStarter.Math.TransformStatics.VectorLibrary;

[RequireComponent(typeof(InputManager3D))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CameraController3D))]
public class Player3D : BasePlayer
{
    #region Serialized Fields
    [Header("Reference")]
    [SerializeField] private Transform controlSource;
    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;
    [SerializeField] private bool instantTurn = false;

    [Header("Jump")]
    [SerializeField, Min(0.1f)] private float jumpHeight = 1.5f;
    [SerializeField, Min(0f)] private float gravityValue = 9.81f;
    [SerializeField, Min(0f)] private float jumpGravityScale = 1f;
    [SerializeField, Min(0f)] private float fallingGravityScale = 1f;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;
    #endregion

    #region References
    private InputManager3D _inputManager;
    private CharacterController _motionController;
    private CameraController3D _cameraController;
    #endregion

    #region Private Fields
    private Quaternion _desiredRotation;
    private bool _canRotate = false;
    private bool _isGrounded = false;
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
    }

    protected override void OnUpdate()
    {
        HandleControlSource();
        HandleGrounded();
    }

    protected override void OnMotionUpdate()
    {
        HandleJumpVelocity();
        HandleGravityVelocity();
        HandleRotation();

        _motionController.Move(PlayerVelocity * Time.deltaTime);
    }

    protected override void OnMoveVelocityHandled(MovementLibrary.MovementResult result)
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

    #region Jump Velocity
    private void HandleGrounded() 
    {
        if (!EnableMotion) return;
        
        _isGrounded = _motionController.isGrounded;

        if (_isGrounded && PlayerVelocity.y < 0)
            SetPlayerVelocityParam(VectorParam.Y, 0);
    }

    private void HandleJumpVelocity() 
    {
        if (_isGrounded && _inputManager.Jump) 
        {
            float jumpMagnitude = Mathf.Sqrt(jumpHeight * gravityValue * (jumpGravityScale + 1));
            SetPlayerVelocityParam(VectorParam.Y, jumpMagnitude);
        }
    }

    private void HandleGravityVelocity() 
    {
        float gravityMagnitude = -gravityValue * fallingGravityScale * Time.deltaTime;
        AddPlayerVelocityParam(VectorParam.Y, gravityMagnitude);
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
}