using UnityEditorInternal;
using UnityEngine;
using UnityGameStarter.CommonData;
using UnityGameStarter.Events.EventManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay
{
    [RequireComponent(typeof(GameStateMachine))]
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameplayData data;
        
        private GameStateMachine stateMachine;

        public GameplayData Data => data;

        public bool IsPaused => stateMachine.CurrentState.GetType() == typeof(PauseState);

        protected override void Awake()
        {
            base.Awake();

            stateMachine = GetComponent<GameStateMachine>();
            stateMachine.SetState(typeof(InitialGameState));

            if (data != null)
                EventManager.Instance.Publish(new RuntimeScale(data.gameTimeScale));
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

        public void EnterGame() 
        {
            stateMachine.SetState(typeof(GameplayState));
        }

        public void RestartGame() 
        {
            stateMachine.reason = ExitGameReason.Restart;
            stateMachine.SetState(typeof(ExitGameState));
        }

        public void LeaveGame() 
        {
            stateMachine.reason = ExitGameReason.Exit;
            stateMachine.SetState(typeof(ExitGameState));
        }
    }
}