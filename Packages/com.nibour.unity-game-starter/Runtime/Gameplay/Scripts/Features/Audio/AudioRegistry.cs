using UnityEngine;

namespace UnityGameStarter.Gameplay.Audio 
{   
    public sealed class AudioRegistry : MonoBehaviour
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