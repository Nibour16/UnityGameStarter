using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.EventSystem.EventManagement;

namespace UnityGameStarter.TestOnlyScripts.Event
{
    [RequireComponent(typeof(EventListenerRegister))]
    public class TestAutoEventListener : MonoBehaviour, IAutoEventListener
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