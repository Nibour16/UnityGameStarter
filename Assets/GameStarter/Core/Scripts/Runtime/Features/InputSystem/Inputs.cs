using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityGameStarter.InputSystem
{
    public class Inputs : MonoBehaviour
    {
        [SerializeField] private InputData[] inputData;
        public InputMode inputMode = InputMode.GameAndUI;

        private void OnEnable()
        {
            foreach (var data in inputData)
                SetActionEnable(data.inputActionReference, true);
        }

        private void OnDisable()
        {
            foreach (var data in inputData)
                SetActionEnable(data.inputActionReference, false);
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
            if (IsDataValid(data, out var action))

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
            
            action = data.inputActionReference;

            if (action == null)
            {
                Debug.LogWarning("Target action is null", this);
                return false;
            }

            return true;
        }

        private void SetActionEnable(InputAction action, bool enable) 
        {
            if (action == null) 
            {
                Debug.LogError("Input Action is null", this);
                return;
            }

            if (enable)
                action.Enable();
            else
                action.Disable();
        }
    }
}