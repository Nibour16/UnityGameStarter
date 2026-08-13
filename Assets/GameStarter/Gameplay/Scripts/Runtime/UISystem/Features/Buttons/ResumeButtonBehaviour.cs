namespace UnityGameStarter.Gameplay.UI.Button 
{
    public class ResumeButtonBehaviour : ButtonBehaviour
    {
        public override void OnPointerClicked()
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