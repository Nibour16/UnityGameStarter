using UnityEngine;
using UnityGameStarter.CursorStatics;

namespace UnityGameStarter.Gameplay.Camera.Spatial3D 
{
    public class CameraController3D : MonoBehaviour, ICameraController
    {
        [SerializeField] private CameraSettings setting;
        [SerializeField] private CameraRuntimeState state;

        private Transform _camera;

        public bool IsMainCamera => true;

        public UnityEngine.Camera Camera 
        {
            get => _camera.GetComponent<UnityEngine.Camera>();
            set => _camera = value.transform;
        }

        public Quaternion CameraRotation => _camera.rotation;

        private void OnEnable()
        {
            CursorLibrary.Lock();
        }

        private void OnDisable()
        {
            CursorLibrary.Unlock();
        }

        public void UpdateCamera(Transform targetSource, Vector2 lookInput)
        {
            if (targetSource == null)
            {
                Debug.LogError("Camera Controller: target source is not valid");
                return;
            }

            state.targetPos = targetSource.position;

            CameraControlLibrary.CalculateCameraTransform(ref _camera, lookInput, ref state, setting);
        }
    }
}