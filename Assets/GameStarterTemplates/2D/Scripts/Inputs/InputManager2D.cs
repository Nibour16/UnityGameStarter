public class InputManager2D : InputManager
{
    private PlayerInputs2D _playerInputs;
    public PlayerInputs2D PlayerInputs => _playerInputs;

    protected override void Awake()
    {
        base.Awake();
        _playerInputs = new PlayerInputs2D();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerInputs?.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerInputs?.Disable();
    }
}