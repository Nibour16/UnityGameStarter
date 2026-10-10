using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Gameplay.PhysicsStatics;

namespace UnityGameStarter.Gameplay.Character.JumpModule
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyJumpModule : BaseRigidbodyJumpModule<Rigidbody, Collider> { }
}