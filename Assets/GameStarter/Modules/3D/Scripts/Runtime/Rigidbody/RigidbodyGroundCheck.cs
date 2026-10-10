using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    [RequireComponent(typeof(SphereCollider))]
    public class RigidbodyGroundCheck : MonoBehaviour
    {
        [SerializeField] private RigidbodyJumpModule jumpModule;
        [SerializeField] private float maxCheckDistance = 5f;

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