using UnityEngine;
using UnityGameStarter.CursorStatics;
using UnityGameStarter.InputSystem.Player.Player_3D;

namespace UnityGameStarter.Gameplay.Camera.Spatial3D
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private Transform targetSource;

        public Transform TargetSource => targetSource;

        [SerializeField] private CameraSettings setting;
        [SerializeField] private CameraRuntimeState state;

        public Quaternion CameraRotation => camera.rotation;

        private void LateUpdate()
        {
            state.targetPos = targetSource.position;

            CameraControlLibrary.CalculateCameraTransform(
                ref camera, InputFacade3D.Instance.Look, ref state, setting);

            targetSource.rotation = camera.rotation;
            CursorLibrary.Lock();
        }
    }
}