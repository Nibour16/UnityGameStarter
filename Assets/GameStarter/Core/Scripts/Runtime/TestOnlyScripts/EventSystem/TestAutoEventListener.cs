using UnityEngine.InputSystem;
using UnityGameStarter.EventSystem.EventManagement;
using UnityGameStarter.EventSystem.EventManagement.AutoEventFeature;

namespace UnityGameStarter.TestOnlyScripts.Event
{
    public class TestAutoEventListener : AutoEventListener<TestEvent>
    {
        private TestEvent _testEvent;
        
        [EventListener]
        private void OnTestEvent(TestEvent e)
        {
            e.ExecuteEvent();
        }

        private void Awake() 
        {
            _testEvent = new TestEvent();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                EventManager.Instance.Publish(_testEvent);
            }
        }
    }
}