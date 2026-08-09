using UnityEngine;

public class InputManager2D : MonoBehaviour
{
    private PlayerInputs_Core _coreInputs;
    public PlayerInputs_Core CoreInputs => _coreInputs;

    private PlayerInputs2D _playerInputs;
    public PlayerInputs2D PlayerInputs => _playerInputs;

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