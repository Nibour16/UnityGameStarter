using UnityEngine;
using UnityGameStarter.SceneManagement;

namespace UnityGameStarter.Gameplay.UI.Button 
{
    public class OpenLevelButtonBehaviour : ResumeButtonBehaviour
    {
        [SerializeField] private string levelName = "NewScene";
        
        public override void OnPointerClicked()
        {
            base.OnPointerClicked();

            if (GameManager.TryGetInstance(out var instance))
                instance.NextLevel(levelName);
            else
                SceneFacade.Instance.Load(levelName);
        }
    }
}