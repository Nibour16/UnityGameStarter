using UnityEngine;
using UnityGameStarter.TimerSystem;

namespace UnityGameStarter.NewUpdateSystem 
{
    /// <summary>
    /// Base monobehaviour class for logic that requires customized update frequency,
    /// like expensive logic that needs to be updated less frequently.
    /// </summary>
    /// 
    /// <remarks>
    /// This class used the timer system, see <see cref="TimerManager"/>
    /// </remarks>
    public abstract class LazyUpdatable : MonoBehaviour
    {
        /// <summary>
        /// Interval between the update, higher the value will make the update less frequently, 
        /// 0 means update per frame
        /// </summary>
        [SerializeField, Min(0f)] private float interval = 0.02f;

        protected virtual string TimerName => "Default Lazy Update Timer";
        protected Timer Timer { get; private set; }
        
        protected virtual void Awake()
        {
            Timer = new Timer(this, interval, TimerName, TimerType.Loopable);
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