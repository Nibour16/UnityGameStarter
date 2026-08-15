using UnityEngine;
using UnityGameStarter.SingletonPattern;

namespace UnityGameStarter.Gameplay.Audio 
{
    public class AudioRegistry : Singleton<AudioRegistry>
    {
        [SerializeField] private Audio[] musicList, sfxList;

        protected override void Awake()
        {
            base.Awake();
            EnableDontDestroyOnLoad();
        }

        private void OnEnable() 
        {
            foreach (var music in musicList)
                AudioManager.RegisterAudio(music);

            foreach (var sfx in sfxList)
                AudioManager.RegisterAudio(sfx);
        }

        private void OnDisable()
        {
            foreach (var music in musicList)
                AudioManager.UnregisterAudio(music);

            foreach (var sfx in sfxList)
                AudioManager.UnregisterAudio(sfx);
        }
    }
}