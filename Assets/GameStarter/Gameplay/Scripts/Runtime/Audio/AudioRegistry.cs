using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.Audio 
{   
    public class AudioRegistry : Singleton<AudioRegistry>
    {
        [SerializeField] private Audio[] audioList;

        private void OnEnable() 
        { 
            foreach (var audio in audioList)
                AudioManager.RegisterAudio(audio);
        }

        private void OnDisable()
        {
            foreach (var audio in audioList)
                AudioManager.UnregisterAudio(audio);
        }
    }
}