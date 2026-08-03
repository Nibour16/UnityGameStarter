namespace UnityGameStarter.Gameplay.UI 
{
    public interface IUIComponent
    {
        void Init();
        void Deinit();
        void OnOpened();
        void OnClosed();
    }
}