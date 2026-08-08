using UnityEngine;
using UnityGameStarter.InputSystem;
using UnityGameStarter.ServiceLocatorPattern.FacadeModule;


public sealed class InputFacade3D : BaseSingletonFacade
    <InputFacade3D, IInputService, InputManager_3DStarter>, IInputService
{
    public Vector2 Movement
        => Manager.PlayerInputs.Player.Move.ReadValue<Vector2>();

    public Vector2 Look
        => Manager.PlayerInputs.Player.Look.ReadValue<Vector2>();

    public bool Jumped
        => Manager.PlayerInputs.Player.Jump.triggered;

    public bool Pause 
        => Manager.Core.Player.OpenPauseMenu.triggered;

    protected override void Awake()
    {
        base.Awake();
        EnableDontDestroyOnLoad();
    }
}

