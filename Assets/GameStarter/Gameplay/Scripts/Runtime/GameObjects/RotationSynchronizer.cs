using UnityEngine;
using UnityGameStarter.Math.TransformStatics;
using UnityGameStarter.NewUpdateSystem;
using static UnityGameStarter.Math.TransformStatics.QuaternionLibrary;

namespace UnityGameStarter.Gameplay.Object 
{   
    /// <summary>
    /// A monoBehaviour component that syncronize all controlled transforms to the same rotation
    /// </summary>
    public class RotationSynchronizer : CustomUpdatable
    {
        [Header("General")]
        [SerializeField] private Transform[] controlTransforms;

        [Header("Settings")]
        [SerializeField] private bool controlPitch = true;
        [SerializeField] private bool controlYaw = true;
        [SerializeField] private bool controlRoll = true;

        [Header("Advanced Settings")]
        [SerializeField] private bool syncDuringStart = false;
        [SerializeField] private UpdateType syncUpdateType = UpdateType.NoUpdate;

        public override UpdateType SyncUpdateType 
        {
            get => syncUpdateType;
            protected set => syncUpdateType = value;
        }

        private void Start() 
        {
            if (syncDuringStart)
                SyncRotation();
        }

        protected override void OnUpdate() => SyncRotation();

        public void SyncRotationManually()
        {
            if (syncUpdateType == UpdateType.NoUpdate)
                SyncRotation();
        }

        private void SyncRotation()
        {
            foreach (var controlTrans in controlTransforms)
            {
                if (controlPitch)
                    controlTrans.rotation = controlTrans.rotation.SetEulerComponent(EulerAxis.X, transform);
                if (controlYaw)
                    controlTrans.rotation = controlTrans.rotation.SetEulerComponent(EulerAxis.Y, transform);
                if (controlRoll)
                    controlTrans.rotation = controlTrans.rotation.SetEulerComponent(EulerAxis.Z, transform);
            }
        }
    }
}