using UnityEngine;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Gameplay;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.PauseManagement;
using UnityGameStarter.Gameplay.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraController3D))]
[RequireComponent(typeof(EventListenerRegister))]
public class Player3D : MonoBehaviour, IAutoEventListener
{
    #region Settings
    [Header("References")]
    [SerializeField] private InputManager3D targetInputManager;
    [SerializeField] private Transform controlSource;

    [Header("Movement")]
    [SerializeField, Min(0.01f)] private float moveSpeed = 2f;
    //[SerializeField, Min(0.01f)] private float jumpIntensity = 1f;
    [SerializeField, Min(0.01f)] private float turnSpeed = 10f;

    [Header("Player Setting")]
    [SerializeField] private bool useControlRotation = true;
    #endregion

    #region References
    private Rigidbody _rb;
    private CameraController3D _cameraController;
    #endregion

    #region References of the inputs in InputManager
    private Vector2 Movement =>
        targetInputManager.PlayerInputs.Player.Move.ReadValue<Vector2>();

    private Vector2 Look =>
        targetInputManager.PlayerInputs.Player.Look.ReadValue<Vector2>();

    private bool Jump =>
        targetInputManager.PlayerInputs.Player.Jump.triggered;

    private bool Pause =>
        targetInputManager.CoreInputs.Player.OpenPauseMenu.triggered;
    #endregion

    #region Life Cycle
    private void Awake() 
    {
        _rb = GetComponent<Rigidbody>();
        _cameraController = GetComponent<CameraController3D>();
    }

    private void Update() 
    {
        SetPause();
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private void LateUpdate() 
    {
        if (!_cameraController.enabled) return;

        _cameraController.UpdateCamera(controlSource, Look);
        controlSource.rotation = _cameraController.CameraRotation;
    }
    #endregion

    #region Movement Logic
    private void Move()
    {
        if (GameManager.TryGetInstance(out var instance) && instance.IsPaused) return;
        
        Vector3 velocity = _rb.linearVelocity;

        MovementLibrary.CalculateMovementVelocity(
            controlSource, Movement, moveSpeed, ref velocity, out var result, useControlRotation);

        _rb.linearVelocity = velocity;

        if (result.hasRotation) 
        {
            Quaternion rotation = _rb.rotation.GetTurnRotation(
                result.rotation, turnSpeed, Time.fixedDeltaTime);

            _rb.MoveRotation(rotation); 
        }
    }
    #endregion

    #region Pause Handlers
    private void SetPause() 
    {
        if (Pause) 
        {
            if (!GameManager.TryGetInstance(out var instance)) return;

            if (instance.IsPaused)
                UIController.Instance.CloseUI("PauseMenu");
            else
                UIController.Instance.OpenUI("PauseMenu");
        }
    }

    [EventListener]
    private void OnPaused(PauseChangedEvent e) 
    {
        if (e.IsPaused)
            _cameraController.enabled = false;
        else
            _cameraController.enabled = true;
    }
    #endregion
}