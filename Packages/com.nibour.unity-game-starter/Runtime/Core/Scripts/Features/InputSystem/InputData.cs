using UnityEngine.InputSystem;

namespace UnityGameStarter.InputSystem 
{
    public enum InputMode
    {
        Game,
        UI,
        GameAndUI
    }

    [System.Serializable]
    public class InputData
    {
        public string name;
        public InputMode inputType;
        public InputActionReference inputActionReference;
    }

}