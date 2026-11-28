using UnityEngine;
using UnityGameStarter.CursorStatics;
using UnityGameStarter.DoOnceStatics;

namespace UnityGameStarter.Gameplay.Camera.Spatial3D 
{
    public class CameraController3D : MonoBehaviour, ICameraController
    {
        [SerializeField] private CameraSettings setting;
        [SerializeField] private CameraRuntimeState state;

        [Header("Advanced")]
        /// <summary>
        /// TODO: Once multi-player system applied, remove this boolean
        /// </summary>
        [SerializeField] private bool forceLockCursor = true;

        private Transform _camera;
        private string ID => $"{name}_CameraController3D";

        public bool IsMainCamera => true;

        public UnityEngine.Camera Camera 
        {
            get => _camera.GetComponent<UnityEngine.Camera>();
            set => _camera = value.transform;
        }

        public Quaternion CameraRotation => _camera.rotation;

        private void OnEnable()
        {
            gameObject.Lock();
        }

        private void OnDisable()
        {
            gameObject.Unlock();

            DoOnce.Reset(ID); // TODO: Once multi-player system applied, remove this line
        }

        /// <summary>
        /// TODO: Once multi-player system applied, remove this method
        /// </summary>
        private void Update() 
        {
            if (forceLockCursor)
                DoOnce.Run(ID, () => gameObject.Lock());
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