using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameStarter.Events;
using UnityGameStarter.SceneManagement;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.PlayerSystem 
{
    public class PlayerData 
    {
        public readonly string name;

        internal GameObject reference;
        internal Vector3 initialPosition;
        internal Quaternion initialRotation;

        public Vector3 currentPosition;
        public Quaternion currentRotation;

        public GameObject Reference => reference;
        public Vector3 InitialPosition => initialPosition;
        public Quaternion InitialRotation => initialRotation;

        public PlayerData(GameObject reference) 
        {
            this.reference = reference;

            name = reference.name;
            initialPosition = reference.transform.position;
            initialRotation = reference.transform.rotation;
        }
    }

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
        }

        /// <summary>
        /// Add player to the data
        /// </summary>
        public void JoinPlayer(GameObject player) => _players.Add(ConvertToPlayerData(player));

        public PlayerData FindPlayerData(string playerName) => _players.Find(x => x.name == playerName);
        public PlayerData FindPlayerData(Transform player, bool checkReference = false) 
        {
            if (checkReference)
                return _players.Find(x => x.Reference == player.gameObject);

            return _players.Find(x => x.name == player.name);
        }
        public PlayerData FindPlayerData(GameObject player, bool checkReference = false) 
        {
            if (checkReference)
                return _players.Find(x => x.Reference == player);

            return _players.Find(x => x.name == player.name);
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

        private void Initialization(Scene scene, LoadSceneMode mode) 
        {
            if (_initialized) return;

            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
                JoinPlayer(player);

            EventManager.Instance.Publish(new PlayerInitializedEvent(_players));
            _initialized = true;
        }

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
                        data.reference = player;
                    else
                        data.reference = instance.SpawnPlayer(data.InitialPosition, data.InitialRotation);
                }

                if (data.Reference != player) 
                {
                    Destroy(player);
                    continue;
                }

                data.initialPosition = player.transform.position;
                data.initialRotation = player.transform.rotation;
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