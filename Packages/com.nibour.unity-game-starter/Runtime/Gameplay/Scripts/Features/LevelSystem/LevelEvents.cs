using UnityEngine;

namespace UnityGameStarter.Gameplay.LevelManagement 
{
    public abstract class BaseLevelEvent
    {
        // TODO: Add sub-levels collection
        public BaseLevelEvent(/* TODO: add parameters that returns sub levels */)
        {

        }
    }

    public class InitializeLevelEvent : BaseLevelEvent
    {
        public InitializeLevelEvent() : base() { }
    }

    public class InitializeLevelCompleteEvent : BaseLevelEvent
    {
        public InitializeLevelCompleteEvent() : base() { }
    }

    public class EnterLevelEvent : BaseLevelEvent
    {
        public EnterLevelEvent() : base() { }
    }

    public class UpdateLevelEvent : BaseLevelEvent
    {
        public UpdateLevelEvent() : base() { }
    }

    public class ExitLevelEvent : BaseLevelEvent
    {
        public ExitLevelEvent() : base() { }
    }
}
