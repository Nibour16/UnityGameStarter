using System.Collections.Generic;
using UnityEngine;
using UnityGameStarter.Gameplay.PhysicsStatics;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyJumpModule : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float jumpStrength = 6f;
    [SerializeField, Min(0f)] private float gravityScale = 1f;
    [SerializeField, Min(0f)] private float fallingGravityScale = 1f;
    [Space]
    [SerializeField] private LayerMask groundMask = 1 << 0;

    public bool IsGrounded => _groundColliders.Count > 0;

    private Rigidbody _rb;

    private readonly HashSet<Collider> _groundColliders = new();
    private float _currentGravityScale;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentGravityScale = gravityScale;
    }

    public void Jump() 
    {
        if (!IsGrounded) return;

        _rb.ApplyForce(jumpStrength, Vector3.up, ForceMode.Impulse);
    }

    public void ResolveGravity()
    {
        _rb.ApplyExtraGravity(_rb.mass, _currentGravityScale - 1);
    }

    public void UpdateGravityState()
    {
        if (_rb.linearVelocity.y >= 0)
            _currentGravityScale = gravityScale;
        else
            _currentGravityScale = fallingGravityScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsGround(other))
            _groundColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _groundColliders.Remove(other);
    }

    private bool IsGround(Collider other)
        => (groundMask.value & (1 << other.gameObject.layer)) != 0;
}