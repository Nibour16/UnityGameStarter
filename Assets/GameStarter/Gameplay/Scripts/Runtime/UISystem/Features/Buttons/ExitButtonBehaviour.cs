using UnityGameStarter.ApplicationStatics;

namespace UnityGameStarter.Gameplay.UI.Button 
{
    public class ExitButtonBehaviour : ResumeButtonBehaviour
    {
        public override void OnPointerClicked()
        {
            base.OnPointerClicked();

            if (GameManager.TryGetInstance(out var instance))
                instance.LeaveGame();
            else
                ApplicationLibrary.QuitApp();
        }
    }
}