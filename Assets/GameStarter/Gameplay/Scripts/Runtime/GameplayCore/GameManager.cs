using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public GameplayData data;
        
        [SerializeField] private GameStateMachine stateMachine;

        protected override void Awake()
        {
            base.Awake();

            if (!stateMachine) stateMachine = FindAnyObjectByType<GameStateMachine>();

            if (!stateMachine) 
                Debug.LogError(
                    "Game Manager: Cannot find Game State Machine in the scene." +
                    "Please assign it in the inspector or ensure one exists in the scene.");
        }

        public void SaveGame() 
        {
            // TODO: Save the game by using Save Manager
        }
        
        public void LoadGame() 
        {
            stateMachine.SetState(typeof(LoadingState));
        }
        
        public void PauseGame() 
        {
            stateMachine.SetState(typeof(PauseState));
        }

        public void ResumeGame() 
        {
            stateMachine.SetState(typeof(GameplayState));
        }

        public void RestartGame() 
        {
            // TODO: restart the game
        }

        public void LeaveGame() 
        {
            stateMachine.SetState(typeof(ExitGameState));
        }
    }
}