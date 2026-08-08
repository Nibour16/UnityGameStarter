namespace UnityGameStarter.InputSystem.Player_2D 
{
    public class InputManager_2DStarter : InputManager<InputManager_2DStarter>
    {
        private PlayerInputs_2D_Starter _playerInputs;
        public PlayerInputs_2D_Starter PlayerInputs => _playerInputs;

        protected override void Awake()
        {
            base.Awake();
            _playerInputs = new PlayerInputs_2D_Starter();
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