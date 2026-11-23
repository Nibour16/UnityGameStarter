using UnityGameStarter.UI;
using UnityGameStarter.UI.Button;

namespace UnityGameStarter.Gameplay.UI.Button
{
    public class ResumeButtonBehaviour : ButtonBehaviour
    {
        public override void OnButtonClick<T>(T eventData)
        {
            if (OwnerRoot == null || OwnerMenu == null) 
            {
                gameObject.SetActive(false);
                return;
            }

            UIController.Instance.CloseUI(OwnerRoot, OwnerMenu.name, true);
        }
    }
}