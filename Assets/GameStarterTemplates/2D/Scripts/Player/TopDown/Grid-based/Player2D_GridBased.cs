using UnityEngine;
using UnityGameStarter.Gameplay.CharacterMovement;
using UnityGameStarter.Gameplay.GridSystem;
using UnityGameStarter.Gameplay.PlayerSystem;

[RequireComponent(typeof(InputManager2D))]
public class Player2D_GridBased : BasePlayer, IGridSnappable
{
    [Header("Grid")]
    [SerializeField] private SnappableGridManager gridManager;
    [SerializeField] private string[] blockableLayers = { "Wall" };
    [SerializeField] private bool releaseCurrentCellOnMovingStart = false;

    [HideInInspector] public GameObject deadEntity = null;

    private InputManager2D _inputManager;

    private GridBasedMovement _movement;
    public GridBasedMovement Movement => _movement;

    protected override Transform ControlSource => transform;
    protected override bool UseControlRotation => false;

    protected override Vector2 MoveInput => _inputManager.Move;
    protected override bool PauseInput => _inputManager.Pause;

    public override Vector2 MoveDirection2D => _movement.Direction(MoveInput);

    private void OnEnable() 
    {
        if (_movement == null || !_movement.IsValidGrid()) return;
        
        _movement.SnapOwner();
    }

    private void OnDisable()
    {
        if (_movement == null || !_movement.IsValidGrid()) return;

        _movement.ReleaseCurrentCell();
    }

    protected override void OnInitialized()
    {
        _inputManager = GetComponent<InputManager2D>();

        if (!gridManager) 
        {
            Debug.LogError("GridManager is not setup");
            return;
        }

        _movement = new GridBasedMovement(gameObject, gridManager, blockableLayers);
    }

    protected override void OnMoveVelocityHandled(MovementResult result)
    {
        if (_movement == null || !_movement.IsValidGrid()) return;

        _movement.Move(MoveInput, moveSpeed * Time.deltaTime, IsPaused, releaseCurrentCellOnMovingStart);
    }

    public void OnSnapped(string snappedLayer, Vector3Int gridID, Vector3Int coord)
    {
        if(_movement == null || !_movement.IsValidGrid()) return;

        _movement.OnSnapped(snappedLayer, gridID, coord);
    }
}