using UnityEngine;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Gameplay.PlayerSystem;
using UnityGameStarter.UI;

namespace UnityGameStarter.Gameplay.UI.Menu 
{
    public class PauseMenuBehaviour : UIBehaviour
    {
        [SerializeField] private bool pauseGame = true;
        
        public override void OnOpened()
        {
            if (pauseGame) 
            {
                if (GameManager.TryGetInstance(out var instance))
                    instance.PauseGame();
            }
            else
                EventManager.Instance.Publish(new OnPlayerPauseEvent());
        }

        public override void OnClosed()
        {
            if (pauseGame) 
            {
                if (GameManager.TryGetInstance(out var instance))
                    instance.EnterGame();
            }
            else
                EventManager.Instance.Publish(new OnPlayerUnpauseEvent());
        }
    }
}