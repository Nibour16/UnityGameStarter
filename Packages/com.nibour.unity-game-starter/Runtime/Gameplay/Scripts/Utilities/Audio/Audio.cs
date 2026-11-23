using UnityEngine;

namespace UnityGameStarter.Gameplay.Audio 
{
    public enum AudioType
    {
        Music,
        SoundEffect
    }
    
    [System.Serializable]
    public sealed class Audio
    {
        [SerializeField] private string audioName;
        [SerializeField] private AudioType audioType;
        [SerializeField] private AudioSource audioSource;
        
        public string Name => audioName;
        public AudioType Type => audioType;
        public AudioSource Source => audioSource;
    }
}