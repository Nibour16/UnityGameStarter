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

        [Header("UI-less Settings")]
        [SerializeField] private bool pauseGame = true;

        public void SetPause()
        {
            if (!GameManager.TryGetInstance(out var instance)) return;

            if (instance.IsPaused) 
            {
                if (UIController.TryGetInstance(out var ui))
                    if (ui.CloseUI(targetPauseMenuName)) return;

                if (pauseGame)
                    instance.EnterGame();
                else
                    EventManager.Instance.Publish(new OnPlayerUnpauseEvent());
            }
            else 
            {
                if (UIController.TryGetInstance(out var ui))
                    if (ui.OpenUI(targetPauseMenuName)) return;

                if (pauseGame)
                    instance.PauseGame();
                else
                    EventManager.Instance.Publish(new OnPlayerPauseEvent());
            }
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