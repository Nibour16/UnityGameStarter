using UnityEngine;
using UnityGameStarter.ServiceLocatorPattern.FacadeModule;

namespace UnityGameStarter.InputSystem.Player.Player_3D 
{
    public class InputFacade3D : BaseSingletonFacade
        <InputFacade3D, IInputService, InputManager_3DStarter>, IInputService
    {
        public Vector2 Movement
            => Manager.PlayerInputs.Player.Move.ReadValue<Vector2>();

        public Vector2 Look
            => Manager.PlayerInputs.Player.Look.ReadValue<Vector2>();

        public bool Jumped
            => Manager.PlayerInputs.Player.Jump.triggered;

        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();
        }
    }
}

