using UnityEngine;

namespace UnityGameStarter.Gameplay.Interaction 
{
    public class GameplayInteractTrigger2D : BaseGameplayInteractor
    {
        [SerializeField] private bool continuousTrigger = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            FocusTarget(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (continuousTrigger)
                FocusTarget(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UnfocusTarget(other.gameObject);
        }
    }
}