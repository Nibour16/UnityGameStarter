using UnityEngine;
using UnityGameStarter.Math.TransformStatics;
using static UnityGameStarter.Math.TransformStatics.QuaternionLibrary;

namespace UnityGameStarter.Gameplay.Spatial3D 
{
    public class RotationSynchronizer : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform[] controlTransforms;

        [SerializeField] private bool yawOnly = true;

        [Header("Advanced")]
        [SerializeField] private bool syncEveryFrame = true;

        public void SyncRotation()
        {
            if (syncEveryFrame) return;

            ApplyRotation();
        }

        private void ApplyRotation()
        {
            foreach (var controlTrans in controlTransforms)
            {
                if (yawOnly)
                    controlTrans.rotation = controlTrans.rotation.SetEulerComponent(EulerAxis.Y, transform);
                else
                    controlTrans.rotation = transform.rotation;
            }
        }

        private void LateUpdate()
        {
            if (syncEveryFrame)
                ApplyRotation();
        }
    }
}