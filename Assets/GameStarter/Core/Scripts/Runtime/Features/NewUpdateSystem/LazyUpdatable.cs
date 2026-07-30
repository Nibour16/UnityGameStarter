using UnityEngine;
using UnityGameStarter.TimerSystem;

namespace UnityGameStarter.NewUpdateSystem 
{
    public abstract class LazyUpdatable : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float interval = 0.02f;
        [SerializeField] private string timerName = "Default Timer";

        protected Timer Timer { get; private set; }
        
        protected virtual void Awake()
        {
            Timer = new Timer(this, interval, timerName, TimerType.Loopable);
        }

        protected virtual void OnEnable()
        {
            Timer.BindCompleted(LazyUpdate);
            Timer.Start();
        }

        protected virtual void OnDisable()
        {
            Timer.Cancel();
            Timer.UnbindCompleted(LazyUpdate);
        }

        protected abstract void LazyUpdate();
    }
}