namespace UnityGameStarter.InputSystem.Player.Player_3D 
{
    public class InputManager_3DStarter : InputManager<InputManager_3DStarter>
    {
        private PlayerInputs_3D_Starter _playerInputs;
        public PlayerInputs_3D_Starter PlayerInputs => _playerInputs;

        protected override void Awake()
        {
            base.Awake();
            _playerInputs = new PlayerInputs_3D_Starter();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _playerInputs?.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _playerInputs?.Disable();
        }
    }
}