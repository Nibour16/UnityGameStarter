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
            => Instantiate(playerPrefab, position, rotation);
    }
}