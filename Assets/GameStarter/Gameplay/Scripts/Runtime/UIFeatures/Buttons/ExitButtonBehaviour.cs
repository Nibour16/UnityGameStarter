using UnityEngine;
using UnityGameStarter.ApplicationStatics;

namespace UnityGameStarter.Gameplay.UI.Button
{
    public class ExitButtonBehaviour : ResumeButtonBehaviour
    {
        [SerializeField] private bool applicationQuit = false;
        
        public override void OnPointerClicked()
        {
            base.OnPointerClicked();

            if (applicationQuit) 
            {
                ApplicationLibrary.QuitApp();
                return;
            }

            if (GameManager.TryGetInstance(out var instance))
                instance.LeaveGame();
            else
                ApplicationLibrary.QuitApp();
        }
    }
}