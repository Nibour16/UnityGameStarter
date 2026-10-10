using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    public class RigidbodyGroundCheck : BaseRigidbodyGroundCheck<Collider>
    {
        [SerializeField] private RigidbodyJumpModule jumpModule;

        private void OnTriggerEnter(Collider collision)
        {
            jumpModule.EnterTrigger(transform, collision, maxCheckDistance);
        }

        private void OnTriggerStay(Collider collision)
        {
            jumpModule.StayTrigger(transform, collision, maxCheckDistance);
        }

        private void OnTriggerExit(Collider collision)
        {
            jumpModule.ExitTrigger(collision);
        }
    }

}