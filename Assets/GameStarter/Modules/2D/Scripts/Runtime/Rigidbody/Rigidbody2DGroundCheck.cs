using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Rigidbody2DGroundCheck : MonoBehaviour
    {
        [SerializeField] private Rigidbody2DJumpModule jumpModule;
        [SerializeField] private float maxCheckDistance = 5f;

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