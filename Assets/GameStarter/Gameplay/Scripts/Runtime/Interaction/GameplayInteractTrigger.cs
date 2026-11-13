using UnityEngine;

namespace UnityGameStarter.Gameplay.Interaction 
{
    public class GameplayInteractTrigger : BaseGameplayInteractor
    {
        [SerializeField] private bool continuousTrigger = false;
        
        private void OnTriggerEnter(Collider other)
        {
            FocusTarget(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (continuousTrigger)
                FocusTarget(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            UnfocusTarget(other.gameObject);
        }
    }
}