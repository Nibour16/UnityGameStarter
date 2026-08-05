using UnityEngine;

namespace UnityGameStarter.Gameplay.UI.Menu 
{
    [RequireComponent(typeof(UIElement))]
    public class GamePauseMenu : MonoBehaviour, IUIComponent
    {
        public void Init() { }

        public void Deinit() { }

        public void OnOpened()
        {
            GameManager.Instance.PauseGame();
        }

        public void OnClosed()
        {
            GameManager.Instance.ResumeGame();
        }
    }
}