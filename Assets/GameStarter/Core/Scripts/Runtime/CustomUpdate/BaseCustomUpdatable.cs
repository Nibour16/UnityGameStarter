using UnityEngine;
using UnityGameStarter.TimerSystem;

namespace UnityGameStarter.CustomUpdate 
{
    public abstract class BaseCustomUpdatable : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float interval = 1f;
        [SerializeField] private string timerName = "Default Timer";

        protected Timer Timer { get; private set; }
        
        protected virtual void Awake()
        {
            Timer = new Timer(this, interval, timerName, TimerType.Loopable);
        }

        protected virtual void OnEnable()
        {
            Timer.BindCompleted(OnIntervalUpdate);
            Timer.Start();
        }

        protected virtual void OnDisable()
        {
            Timer.Cancel();
            Timer.UnbindCompleted(OnIntervalUpdate);
        }

        protected abstract void OnIntervalUpdate();
    }
}