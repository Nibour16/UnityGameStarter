using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityGameStarter.UI
{
    public class UINavigationInitializer : UIBehaviour
    {
        [SerializeField] private GameObject firstSelected;

        public override void OnOpened()
        {
            var system = EventSystem.current;

            if (!system)
            {
                Debug.LogError("This component requires an event system in the scene to work", this);
                return;
            }

            if (!firstSelected) return;

            system.SetSelectedGameObject(firstSelected);
        }

        public override void OnClosed() { }
    }
}