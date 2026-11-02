using UnityGameStarter.SceneManagement;

namespace UnityGameStarter.Gameplay.UI.Button 
{
    public class RestartButtonBehaviour : ResumeButtonBehaviour
    {
        public override void OnButtonClick<T>(T eventData)
        {
            base.OnButtonClick(eventData);

            if (GameManager.TryGetInstance(out var instance))
                instance.RestartGame();
            else
                SceneFacade.Instance.Reload();
        }
    }
}