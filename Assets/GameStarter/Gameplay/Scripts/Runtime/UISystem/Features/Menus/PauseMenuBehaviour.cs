namespace UnityGameStarter.Gameplay.UI.Menu 
{
    public class PauseMenuBehaviour : UIBehaviour
    {
        public override void OnOpened()
        {
            if (GameManager.TryGetInstance(out var instance))
                instance.PauseGame();
        }

        public override void OnClosed()
        {
            if (GameManager.TryGetInstance(out var instance))
                instance.EnterGame();
        }
    }
}