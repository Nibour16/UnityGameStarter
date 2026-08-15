using UnityEngine;
using UnityEngine.Audio;

namespace UnityGameStarter.Gameplay.Audio 
{
    [System.Serializable]
    public class Audio
    {
        [SerializeField] private AudioClip clip;
        public AudioClip Clip => clip;

        [SerializeField] private AudioMixerGroup mixerGroup;
        public AudioMixerGroup MixerGroup => mixerGroup;

        [SerializeField] private string name;
        public string Name => name;

        [SerializeField, Range(0f, 1f)] private float volume = 1f;
        public float Volume => volume;

        [SerializeField, Min(0.1f)] private float pitch = 1f;

        public float Pitch => pitch;

        [SerializeField] private bool loop = false;
        public bool Loop => loop;

        [SerializeField] private bool playOnAwake = false;
        public bool PlayOnAwake => playOnAwake;
    }
}