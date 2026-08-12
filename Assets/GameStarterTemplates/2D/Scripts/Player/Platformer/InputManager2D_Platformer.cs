using UnityEngine;

public class InputManager2D_Platformer : MonoBehaviour
{
    private PlayerInputs_Core _coreInputs;
    private PlayerInputs2D_Platformer _playerInputs;

    public Vector2 Move =>
        _playerInputs.Player.Move.ReadValue<Vector2>();

    public bool Jump =>
        _playerInputs.Player.Jump.WasPressedThisFrame();

    public bool Pause =>
        _coreInputs.Player.OpenPauseMenu.WasPressedThisFrame();

    private void Awake()
    {
        _coreInputs = new PlayerInputs_Core();
        _playerInputs = new PlayerInputs2D_Platformer();
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