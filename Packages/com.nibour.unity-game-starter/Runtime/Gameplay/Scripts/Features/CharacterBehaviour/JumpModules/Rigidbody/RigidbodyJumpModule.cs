using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyJumpModule : BaseRigidbodyJumpModule<Rigidbody, Collider> { }
}