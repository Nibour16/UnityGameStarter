
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputs_Core _core;
    public PlayerInputs_Core Core => _core;

    protected virtual void Awake()
    {
        _core = new PlayerInputs_Core();
    }

    protected virtual void OnEnable() 
    {
        _core?.Enable();
    }

    protected virtual void OnDisable() 
    {
        _core?.Disable();
    }
}