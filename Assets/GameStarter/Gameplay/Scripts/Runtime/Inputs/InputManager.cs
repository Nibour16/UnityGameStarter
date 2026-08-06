using UnityGameStarter.ServiceLocatorPattern;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.InputSystem 
{
    public interface IInputService : IService { }
    
    public abstract class InputManager<T> : Singleton<T> where T : InputManager<T>
    {
        private PlayerInputs_Core _core;
        public PlayerInputs_Core Core => _core;

        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();

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