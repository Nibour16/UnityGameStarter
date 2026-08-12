using UnityEngine;
using UnityGameStarter.Gameplay.Character.JumpModule.RB2D;
using UnityGameStarter.Gameplay.PlayerSystem;
using static UnityGameStarter.Gameplay.CharacterMovement.MovementLibrary;

[RequireComponent(typeof(InputManager2D_Platformer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Rigidbody2DJumpModule))]
public class Player2D_Platformer : BasePlayer
{
    #region References
    private InputManager2D_Platformer _inputManager;
    private Rigidbody2D _rb;
    private Rigidbody2DJumpModule _jumpModule;
    #endregion

    #region Caches
    private float _cachedGravityScale = 0;
    #endregion

    #region Override Properties
    protected override Transform ControlSource => transform;
    protected override bool UseControlRotation => false;
    protected override Vector2 MoveInput => new (_inputManager.Move.x, 0);
    protected override bool PauseInput => _inputManager.Pause;
    #endregion

    #region Life Cycle
    protected override void OnInitialized()
    {
        _inputManager = GetComponent<InputManager2D_Platformer>();
        _rb = GetComponent<Rigidbody2D>();
        _jumpModule = GetComponent<Rigidbody2DJumpModule>();
    }

    protected override void OnUpdate() 
    {
        HandleJump();
    }
    #endregion

    #region Control Methods
    private void HandleJump()
    {
        if (_jumpModule.isActiveAndEnabled && _inputManager.Jump)
            _jumpModule.Jump();
    }
    protected override void OnMoveVelocityHandled(MovementResult result)
    {
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = PlayerVelocity2D.x;

        _rb.linearVelocity = velocity;
    }
    #endregion

    #region Pause State Handle
    protected override void OnPlayerPaused(OnPlayerPauseEvent e)
    {
        _cachedGravityScale = _rb.gravityScale;

        _rb.gravityScale = 0;
        _jumpModule.enabled = false;
    }

    protected override void OnPlayerUnpaused(OnPlayerUnpauseEvent e)
    {
        _rb.gravityScale = _cachedGravityScale;
        _jumpModule.enabled = true;

        _cachedGravityScale = 0;
    }
    #endregion
}