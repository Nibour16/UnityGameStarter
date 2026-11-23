using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameStarter.Events;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.PlayerSystem 
{
    public class PlayerInitializedEvent 
    {
        private readonly List<PlayerData> _players;
        public IReadOnlyList<PlayerData> Players => _players;

        public PlayerInitializedEvent(List<PlayerData> players) 
        {
            _players = players;
        }
    }

    [RuntimeSingleton]
    public class PlayerManager : Singleton<PlayerManager>
    {
        private readonly List<PlayerData> _players = new();
        private bool _initialized = false;

        /// <summary>
        /// Collect all players in the scene during initialization
        /// </summary>
        protected override void Awake() 
        {
            base.Awake();

            SceneFacade.Instance.BindSceneLoaded(Initialization);
            SceneFacade.Instance.BindSceneLoaded(LoadPlayers);
            SceneFacade.Instance.BindSceneUnloadStart(UpdatePlayerDuringUnload);
        }

        /// <summary>
        /// Add player to the data
        /// </summary>
        public void JoinPlayer(GameObject player) => _players.Add(ConvertToPlayerData(player));

        public PlayerData FindPlayerData(string playerName) => _players.Find(x => x.Name == playerName);
        public PlayerData FindPlayerData(Transform player, bool checkReference = false) 
        {
            if (checkReference)
                return _players.Find(x => x.Reference == player.gameObject);

            return _players.Find(x => x.Name == player.name);
        }
        public PlayerData FindPlayerData(GameObject player, bool checkReference = false) 
        {
            if (checkReference)
                return _players.Find(x => x.Reference == player);

            return _players.Find(x => x.Name == player.name);
        }

        /// <summary>
        /// Remove player from the data
        /// </summary>
        public bool RemovePlayer(GameObject player) => _players.Remove(ConvertToPlayerData(player));

        /// <summary>
        /// Update current player position and rotation
        /// The data is still valid even if the player object is destroyed
        /// Usually for when a specific player is destroyed or a code is trying to access them
        /// </summary>
        public void UpdatePlayer(Transform player) 
        {
            var data = FindPlayerData(player);

            if (data == null) 
            {
                Debug.LogError("The player is not found", this);
                return;
            }

            data.currentPosition = player.position;
            data.currentRotation = player.rotation;
        }

        /// <summary>
        /// Update player current transform before unload the scene
        /// </summary>
        private void UpdatePlayerDuringUnload(Scene scene) 
        {
            foreach (var player in _players)
                UpdatePlayer(player.Reference.transform);
        }

        /// <summary>
        /// Initialize the data to collect all existing players in during the game start
        /// </summary>
        private void Initialization(Scene scene, LoadSceneMode mode) 
        {
            if (_initialized) return;

            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
                JoinPlayer(player);

            // Won't be counted as initialized if no player is found. For single player games you don't need any extra setup
            if (_players.Count <= 0) return;

            EventManager.Instance.Publish(new PlayerInitializedEvent(_players));
            _initialized = true;
        }

        /// <summary>
        /// Load players to ensure all players that registered in the data are exist in the scene
        /// Only execute when initialization is done.
        /// Will remove all players that are not registered in the data. Usually for preventing duplication
        /// </summary>
        private void LoadPlayers(Scene scene, LoadSceneMode mode) 
        {
            if (!_initialized) return;
            
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players) 
            {
                var data = FindPlayerData(player);

                if (data == null) 
                {
                    Destroy(player);
                    continue;
                }

                if (data.Reference == null)
                {
                    if (!PlayerSpawner.TryGetInstance(out var instance, true))
                        data.SetReference(player);
                    else
                        data.SetReference(instance.SpawnPlayer(data.InitialPosition, data.InitialRotation));
                }
                else 
                {
                    // TODO: For now just return the player to the initial position
                    data.ResetPlayerToInitial();
                }

                if (data.Reference != player) 
                {
                    Destroy(player);
                    continue;
                }
            }
        }

        private PlayerData ConvertToPlayerData(GameObject player)
        {
            if (!player.CompareTag("Player"))
            {
                Debug.LogError("Only gameObject with player tag can join", this);
                return null;
            }

            return new PlayerData(player);
        }

    }
}