using UnityEngine;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Gameplay.UI;
using UnityGameStarter.PauseManagement;

namespace UnityGameStarter.Gameplay.PlayerSystem
{
    public struct OnPlayerPauseEvent { }
    public struct OnPlayerUnpauseEvent { }

    [RequireComponent(typeof(EventListenerRegister))]
    public class PlayerController : MonoBehaviour, IAutoEventListener
    {
        [SerializeField] private string targetPauseMenuName = "PauseMenu";
        
        public void SetPause()
        {
            if (!GameManager.TryGetInstance(out var instance)) return;

            if (instance.IsPaused)
                UIController.Instance.CloseUI(targetPauseMenuName);
            else
                UIController.Instance.OpenUI(targetPauseMenuName);
        }

        [EventListener]
        private void OnPauseChanged(PauseChangedEvent e)
        {
            if (e.IsPaused) 
            {
                EventManager.Instance.Publish(new OnPlayerPauseEvent());
            }
            else 
            {
                EventManager.Instance.Publish(new OnPlayerUnpauseEvent());
            }
        }
    }
}