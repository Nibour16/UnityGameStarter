using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameStarter.EventSystem.EventManagement;

namespace UnityGameStarter.TestOnlyScripts 
{
    public class TestEvent : MonoBehaviour
    {
        public void ExecuteEvent()
        {
            Debug.Log("TestEventApplicant: 'Hello world!'");
        }

        private void Update() 
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                EventManager.Instance.Publish(this);
            }
        }
    }
}