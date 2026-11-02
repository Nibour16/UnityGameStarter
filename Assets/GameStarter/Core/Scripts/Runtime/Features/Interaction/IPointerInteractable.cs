namespace UnityGameStarter.Interaction 
{
    public interface IPointerInteractable
    {
        void OnHovered();
        void OnUnhovered();

        void OnPointerPressed();
        void OnPointerReleased();

        void OnPointerClicked();
    }
}

