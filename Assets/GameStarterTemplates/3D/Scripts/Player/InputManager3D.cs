public class InputManager3D : InputManager
{
    private PlayerInputs3D _playerInputs;
    public PlayerInputs3D PlayerInputs => _playerInputs;

    protected override void Awake()
    {
        base.Awake();
        _playerInputs = new PlayerInputs3D();
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