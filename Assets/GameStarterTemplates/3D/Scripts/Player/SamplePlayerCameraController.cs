using UnityEngine;
using UnityGameStarter.Gameplay.Camera.Spatial3D;

public class SamplePlayerCameraController : CameraController
{
    protected override Vector2 LookInput => InputFacade3D.Instance.Look;
}
