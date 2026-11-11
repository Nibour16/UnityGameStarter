using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameStarter.CommonData;
using UnityGameStarter.Events;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay
{
    [RequireComponent(typeof(GameStateMachine))]
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameplayData data;
        private GameStateMachine _stateMachine;

        public GameplayData Data => data;
        public bool IsPaused => _stateMachine.CurrentState.GetType() == typeof(PauseState);

        protected override void Awake()
        {
            base.Awake();

            _stateMachine = GetComponent<GameStateMachine>();
            _stateMachine.SetState(typeof(InitialGameState));

            if (data != null)
                EventManager.Instance.Publish(new RuntimeScale(data.gameTimeScale));
        }

        protected override void OnApplicationQuit()
        {
            LeaveGame();
            base.OnApplicationQuit();
        }

        /*public void SaveGame() 
        {
            // TODO: Save the game by using Save Manager
        }*/
        
        public void LoadGame() 
        {
            _stateMachine.SetState(typeof(LoadingState));
        }

        public void PauseGame() 
        {
            _stateMachine.SetState(typeof(PauseState));
        }

        public void EnterGame() 
        {
            _stateMachine.SetState(typeof(GameplayState));
        }

        public void NextLevel(Scene newLevel)
        {
            _stateMachine.reason = ExitGameReason.OpenNewLevel;
            _stateMachine.nextTargetScene = newLevel;
            _stateMachine.SetState(typeof(ExitGameState));
        }

        public void NextLevel(string levelName)
            => NextLevel(SceneFacade.Instance.GetSceneByName(levelName));

        public void RestartGame() 
        {
            _stateMachine.reason = ExitGameReason.Restart;
            _stateMachine.SetState(typeof(ExitGameState));
        }

        public void LeaveGame() 
        {
            _stateMachine.reason = ExitGameReason.Exit;
            _stateMachine.SetState(typeof(ExitGameState));
        }
    }
}