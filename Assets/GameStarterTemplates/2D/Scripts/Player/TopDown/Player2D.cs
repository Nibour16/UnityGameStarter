using UnityEngine;
using UnityGameStarter.Gameplay.PlayerSystem;
using UnityGameStarter.Gameplay.CharacterMovement;

[RequireComponent(typeof(InputManager2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player2D : BasePlayer
{
    #region References
    private InputManager2D _inputManager;
    private Rigidbody2D _rb;
    #endregion

    #region Override Properties
    protected override Transform ControlSource => transform;
    protected override bool UseControlRotation => false;

    protected override Vector2 MoveInput => _inputManager.Move;
    protected override bool PauseInput => _inputManager.Pause;
    #endregion

    #region Implementation
    protected override void OnInitialized()
    {
        _inputManager = GetComponent<InputManager2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnMoveVelocityHandled(MovementResult result)
    {
        _rb.linearVelocity = PlayerVelocity2D;
    }
    #endregion
}
