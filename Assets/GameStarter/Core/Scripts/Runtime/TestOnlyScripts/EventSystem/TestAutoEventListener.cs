using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.EventSystem.EventManagement.AutoEventFeature;

namespace UnityGameStarter.TestOnlyScripts.Event
{
    public class TestAutoEventListener : AutoEventListener<TestEvent>
    {
        [EventListener]
        private void OnTestEvent(TestEvent e)
        {
            e.ExecuteEvent();
        }
    }
}