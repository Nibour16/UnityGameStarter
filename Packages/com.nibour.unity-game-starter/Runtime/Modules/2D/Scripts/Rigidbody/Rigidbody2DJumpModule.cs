using UnityEngine;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DJumpModule : BaseRigidbodyJumpModule<Rigidbody2D, Collider2D> { }
}