using UnityEngine;
using UnityGameStarter.InputSystem;

[RequireComponent(typeof(Inputs))]
public class InputManager : MonoBehaviour
{
    private Inputs _inputs;

    // Properties of all tracking actions
    public Vector2 Move => _inputs.GetAction("Movement")?.ReadValue<Vector2>() ?? Vector2.zero;
    public Vector2 Look => _inputs.GetAction("Look")?.ReadValue<Vector2>() ?? Vector2.zero;
    public bool Jump => _inputs.GetAction("Jump")?.WasPressedThisFrame() ?? false;
    public bool Pause => _inputs.GetAction("Pause")?.WasPressedThisFrame() ?? false;

    // Initialize Inputs
    protected virtual void Awake()
    {
        _inputs = GetComponent<Inputs>();
    }
}