using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    public class Rigidbody2DGroundCheck : BaseRigidbodyGroundCheck<Collider2D>
    {
        [SerializeField] private Rigidbody2DJumpModule jumpModule;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            jumpModule.EnterTrigger(transform, collision, maxCheckDistance);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            jumpModule.StayTrigger(transform, collision, maxCheckDistance);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            jumpModule.ExitTrigger(collision);
        }
    }

}