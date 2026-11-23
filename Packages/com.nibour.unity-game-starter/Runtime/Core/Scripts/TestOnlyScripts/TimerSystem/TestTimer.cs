using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.TimerSystem;

namespace UnityGameStarter.TestOnlyScripts.TimerSystem 
{
    public class TestTimer : MonoBehaviour
    {
        private Timer _timer;
        private bool _isPaused = false;

        [SerializeField] private float timerDuration = 3f;
        [SerializeField] private TimerType timerType;

        private void Awake() 
        {
            _timer = new Timer(this, timerDuration, "Test Timer", timerType);
        }

        private void OnEnable() 
        {
            TimerManager.Instance.Register(_timer);
            _timer.BindRemoved(OnTimerRemoved);
        }

        private void OnDisable() 
        {
            if (_timer.RemainingTime > 0)
                _timer.Cancel();

            TimerManager.Instance.Unregister(_timer);
            _timer.UnbindRemoved(OnTimerRemoved);
        }

        private void OnTimerRemoved() => Debug.Log("Test Timer: removed");
        
        private void Start() 
        {
            Debug.Log("Test Timer: attempt to start");
            _timer.Start
            (
                () => { Debug.Log("Test Timer: complete"); }, 
                () => { Debug.Log("Test Timer: cancelled"); }
            );
        }

        private void Update() 
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame) 
            {
                if (_isPaused) 
                {
                    Debug.Log("Test Timer: attempt to resume");
                    _timer.Resume();
                }
                else 
                {
                    Debug.Log("Test Timer: attempt to pause");
                    _timer.Pause();
                }

                _isPaused = !_isPaused;
            }

            if (Keyboard.current.f2Key.wasPressedThisFrame)
                _timer.Cancel();

            if (Keyboard.current.f3Key.wasPressedThisFrame) 
            {
                Debug.Log("Test Timer: attempt to restart");
                _timer.Restart();
            }
        }
    }
}