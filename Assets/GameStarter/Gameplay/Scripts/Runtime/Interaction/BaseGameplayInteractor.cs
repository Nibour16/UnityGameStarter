using UnityEngine;

namespace UnityGameStarter.Gameplay.Interaction 
{
    public abstract class BaseGameplayInteractor : MonoBehaviour
    {
        private GameObject _focusingTarget;
        private GameObject _interactingTarget;

        protected GameObject FocusingTarget => _focusingTarget;
        protected GameObject InteractingTarget => _interactingTarget;

        public bool InteractInput { private get; set; }

        protected virtual void Update()
        {
            if (_focusingTarget == null) 
            {
                if (_interactingTarget != null)
                    CancelInteract();

                return;
            }

            if (_interactingTarget == null) 
            {
                if (InteractInput)
                    Interact(_focusingTarget);
            }
            else if (!InteractInput)
                CancelInteract();
        }

        private void Interact(GameObject target) 
        {
            if (!target.TryGetComponent<IGameplayInteractable>(out var interactable))
                return;

            _interactingTarget = target;
            interactable?.OnInteract();
        }

        private void CancelInteract()
        {
            if (_interactingTarget == null) return;

            if (_interactingTarget.TryGetComponent<IGameplayInteractable>(out var interactable))
                interactable.OnInteractCancelled();

            _interactingTarget = null;
        }

        protected void FocusTarget(GameObject target) 
        {
            if (_focusingTarget != null || !target.TryGetComponent<IGameplayInteractable>(out var interactable)) 
                return;

            _focusingTarget = target;
            interactable.OnFocused();
        }

        protected void UnfocusTarget(GameObject target) 
        {
            if (_focusingTarget != target) return;

            _focusingTarget = null;

            if (_interactingTarget == target)
                CancelInteract();

            if (target.TryGetComponent<IGameplayInteractable>(out var interactable))
                interactable.OnUnfocused();
        }
    }
}