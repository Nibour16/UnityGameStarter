using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityGameStarter.InputSystem
{
    public class Inputs : MonoBehaviour
    {
        [SerializeField] private InputData[] inputData;
        public InputMode inputMode = InputMode.GameAndUI;

        private InputActionAsset _asset;

        private void Awake() 
        {
            InputActionAsset sourceAsset = null;

            foreach (var data in inputData)
            {
                if (data == null || data.inputActionReference == null) continue;

                var asset = data.inputActionReference.asset;

                // Since first data check did not setup the source asset,
                // therefore it means that it checks if the data are all refer to the first one
                if (sourceAsset == null)
                {
                    sourceAsset = asset;
                    continue;
                }

                if (sourceAsset != asset)
                {
                    Debug.LogError("All InputActionReferences must belong to the same InputActionAsset.", this);
                    return;
                }
            }

            if (sourceAsset == null)
            {
                Debug.LogError("No valid InputActionAsset found.", this);
                return;
            }

            _asset = Instantiate(sourceAsset);
        }

        private void OnEnable()
        {
            if (_asset == null) return;

            _asset.Enable();
        }

        private void OnDisable()
        {
            if (_asset == null) return;

            _asset.Disable();
        }

        private void OnDestroy()
        {
            if (_asset == null) return;

            Destroy(_asset);
            _asset = null;
        }

        public InputAction GetAction(string actionName) 
        {
            var data = inputData.FirstOrDefault(x => x.name == actionName);
            return GetAction(data);
        }

        public InputAction GetAction(int actionID) 
        {
            if (actionID < 0 || actionID >= inputData.Length)
            {
                Debug.LogError($"Input action ID: {actionID} is not a valid ID", this);
                return null;
            }

            var data = inputData[actionID];
            return GetAction(data);
        }

        public InputAction GetAction(InputData data) 
        {
            if (!IsDataValid(data, out var action)) return null;

            if (inputMode == InputMode.GameAndUI || data.inputType == InputMode.GameAndUI)
                return action;

            return inputMode == data.inputType? action : null;
        }

        private bool IsDataValid(InputData data, out InputAction action) 
        {
            action = null;

            if (data == null)
            {
                Debug.LogError("Data is not valid", this);
                return false;
            }

            if (data.inputActionReference == null)
            {
                Debug.LogWarning("Input Action Reference is null", this);
                return false;
            }

            if (_asset == null)
            {
                Debug.LogError("Instance of the Input Action Asset is not generated", this);
                return false;
            }

            var originalAction = data.inputActionReference.action;

            if (originalAction == null)
            {
                Debug.LogWarning("Target action is null", this);
                return false;
            }

            action = _asset.FindAction(originalAction.id);

            if (action == null)
            {
                Debug.LogWarning($"Runtime action not found: {originalAction.name}", this);
                return false;
            }

            return true;
        }
    }
}