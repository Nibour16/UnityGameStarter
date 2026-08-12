using UnityEngine;

public class InputManager2D : MonoBehaviour
{
    private PlayerInputs_Core _coreInputs;
    private PlayerInputs2D _playerInputs;

    public Vector2 Move =>
        _playerInputs.Player.Move.ReadValue<Vector2>();

    public bool Pause =>
        _coreInputs.Player.OpenPauseMenu.WasPressedThisFrame();

    private void Awake()
    {
        _coreInputs = new PlayerInputs_Core();
        _playerInputs = new PlayerInputs2D();
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