namespace UnityGameStarter.Gameplay.Interaction 
{
    public interface IGameplayInteractable
    {
        void OnFocused();
        void OnUnfocused();

        void OnInteract();
        void OnInteractCancelled();
    }
}