using UnityEngine;
using UnityGameStarter.Math.TransformStatics;

namespace UnityGameStarter.Gameplay.PlayerSystem 
{
    public class PlayerData
    {
        public Vector3 currentPosition;
        public Quaternion currentRotation;
        public Vector3 currentScale;

        public string Name { get; private set; }
        public GameObject Reference { get; private set; }

        public Vector3 InitialPosition { get; private set; }
        public Quaternion InitialRotation { get; private set; }
        public Vector3 InitialScale { get; private set; }

        public PlayerData(GameObject reference)
        {
            SetReference(reference);
        }

        public void SetReference(GameObject reference, bool updateReferenceState = true)
        {
            Reference = reference;

            if (!updateReferenceState) return;

            Name = reference.name;
            SetInitialTransformByReference();
        }

        public void ResetPlayerToInitial()
        {
            Reference.transform.SetPositionAndRotation(InitialPosition, InitialRotation);
            Reference.transform.SetLossyScale(InitialScale);
        }

        private void SetInitialTransformByReference()
        {
            InitialPosition = Reference.transform.position;
            InitialRotation = Reference.transform.rotation;
            InitialScale = Reference.transform.lossyScale;
        }
    }
}