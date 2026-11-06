using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityGameStarter.UI
{
    /// <summary>
    /// An initializer for UI Navigation System in specific UI.
    /// This component ensures that the UI's first selected element will be defined correctly by the event system
    /// </summary>
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