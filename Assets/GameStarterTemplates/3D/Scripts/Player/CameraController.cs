using UnityEngine;
using UnityGameStarter.CursorStatics;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.Gameplay.Camera.Spatial3D;

public class CameraController3D : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private CameraSettings setting;
    [SerializeField] private CameraRuntimeState state;

    public Quaternion CameraRotation => camera.rotation;

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
        CameraControlLibrary.CalculateCameraTransform(ref camera, lookInput, ref state, setting);
    }
}