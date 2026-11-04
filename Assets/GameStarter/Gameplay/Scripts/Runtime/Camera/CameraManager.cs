using UnityEngine;
using UnityEngine.SceneManagement;

using UnityGameStarter.SingletonPattern;
using UnityGameStarter.SceneManagement;

namespace UnityGameStarter.Gameplay.Camera 
{
    public interface ICameraController
    {
        bool IsMainCamera { get; }
        UnityEngine.Camera Camera { get; set; }
    }

    /// <summary>
    /// TODO: For now it only offers the main camera, which is incomplete yet
    /// </summary>
    
    [RuntimeSingleton]
    public class CameraManager : Singleton<CameraManager>
    {
        public UnityEngine.Camera MainCamera 
        {
            get 
            {
                var camera = UnityEngine.Camera.main;

                if (camera == null)
                    camera = FindAnyObjectByType<UnityEngine.Camera>();

                return camera;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            SceneFacade.Instance.BindSceneLoaded(OnSceneLoaded);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode e) 
        {
            var cameras = FindObjectsByType<GameObject>(FindObjectsInactive.Include);

            foreach (var camera in cameras) 
            {
                if (!camera.TryGetComponent<ICameraController>(out var iCamera)) continue;

                // TODO: for now all camera properties will be set as main camera, even if the camera is not main camera
                //if (iCamera.IsMainCamera) 

                iCamera.Camera = MainCamera;
            }
        }
    }
}