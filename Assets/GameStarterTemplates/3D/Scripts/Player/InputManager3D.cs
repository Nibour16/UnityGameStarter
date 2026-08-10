using UnityEngine;

public class InputManager3D : MonoBehaviour
{
    private PlayerInputs_Core _coreInputs;
    private PlayerInputs3D _playerInputs;

    public Vector2 Move =>
        _playerInputs.Player.Move.ReadValue<Vector2>();

    public Vector2 Look =>
        _playerInputs.Player.Look.ReadValue<Vector2>();

    public bool Jump =>
        _playerInputs.Player.Jump.WasPressedThisFrame();

    public bool Pause =>
        _coreInputs.Player.OpenPauseMenu.WasPressedThisFrame();

    private void Awake()
    {
        _coreInputs = new PlayerInputs_Core();
        _playerInputs = new PlayerInputs3D();
    }

    private void OnEnable()
    {
        _coreInputs?.Enable();
        _playerInputs?.Enable();
    }

    private void OnDisable()
    {
        _coreInputs?.Disable();
        _playerInputs?.Disable();
    }
}