using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.EventSystem.EventManagement.AutoEventFeature;

namespace UnityGameStarter.TestOnlyScripts 
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