using UnityEngine;
using UnityGameStarter.CursorStatics;

namespace UnityGameStarter.Gameplay.Camera.Spatial3D
{
    public abstract class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private Transform targetSource;

        public Transform TargetSource => targetSource;

        [SerializeField] private CameraSettings setting;
        [SerializeField] private CameraRuntimeState state;

        protected abstract Vector2 LookInput { get; }

        public Quaternion CameraRotation => camera.rotation;

        protected virtual void OnEnable() 
        {
            CursorLibrary.Lock();
        }

        protected virtual void OnDisable() 
        {
            CursorLibrary.Unlock();
        }

        private void LateUpdate()
        {
            UpdateCamera();
        }

        private void UpdateCamera() 
        {
            state.targetPos = targetSource.position;
            CameraControlLibrary.CalculateCameraTransform(ref camera, LookInput, ref state, setting);
            targetSource.rotation = camera.rotation;

            OnCameraUpdate();
        }

        protected virtual void OnCameraUpdate() { }
    }
}