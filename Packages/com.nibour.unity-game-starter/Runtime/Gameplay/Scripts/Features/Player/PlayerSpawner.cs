using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.PlayerSystem 
{
    public class PlayerSpawner : Singleton<PlayerSpawner>
    {
        [SerializeField] private GameObject playerPrefab;

        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();
        }

        public GameObject SpawnPlayer(Vector3 position, Quaternion rotation) 
        {
            var player = Instantiate(playerPrefab, position, rotation);
            player.name = playerPrefab.name;
            return player;
        }
    }
}