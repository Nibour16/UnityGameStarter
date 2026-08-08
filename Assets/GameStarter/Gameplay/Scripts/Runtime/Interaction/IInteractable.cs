namespace UnityGameStarter.Gameplay.Interaction 
{
    public interface IInteractable
    {
        void OnFocused();
        void OnUnfocused();

        void OnInteract();
        void OnInteractCancelled();
    }
}