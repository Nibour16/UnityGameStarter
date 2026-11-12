using UnityEngine;

namespace UnityGameStarter.Gameplay.Interaction 
{
    public class GameplayInteractor : MonoBehaviour
    {
        private GameObject _focusingTarget;
        private GameObject _interactingTarget;

        public GameObject FocusingTarget => _focusingTarget;
        public GameObject InteractingTarget => _interactingTarget;

        public void OnTargetFocused(GameObject target) 
        {
            // TODO: Check if it is default state, false return

            _focusingTarget = target;
            // TODO: Set to focus state
        }

        public void OnTargetUnfocused() 
        {
            // TODO: Check if it is focus state, false return
            
            _focusingTarget = null;
            // TODO: Set to default state
        }

        public void OnTargetInteracted(GameObject target) 
        {
            // TODO: Check if it is focus state, false return
            _focusingTarget = null;
            _interactingTarget = target;

            // TODO: Set to interacted state
        }

        public void OnTargetCancelled() 
        {
            // TODO: Check if it is interacted state, false return

            _interactingTarget = null;
            // TODO: Set to default state
        }
    }
}