namespace UnityGameStarter.InputSystem 
{
    using UnityGameStarter.SingletonPattern;

    public abstract class InputManager<T> : Singleton<T> where T : InputManager<T>
    {
        private PlayerInputs_Core _core;
        public PlayerInputs_Core Core => _core;

        protected override void Awake()
        {
            base.Awake();
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

    public class InputManager : InputManager<InputManager> { }
}