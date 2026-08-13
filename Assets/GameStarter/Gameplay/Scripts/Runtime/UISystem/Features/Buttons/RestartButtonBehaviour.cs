using UnityGameStarter.SceneManagement;

namespace UnityGameStarter.Gameplay.UI.Button 
{
    public class RestartButtonBehaviour : ResumeButtonBehaviour
    {
        public override void OnPointerClicked()
        {
            base.OnPointerClicked();

            if (GameManager.TryGetInstance(out var instance))
                instance.RestartGame();
            else
                SceneFacade.Instance.Reload();
        }
    }
}