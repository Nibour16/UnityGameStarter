using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGameStarter.Gameplay.Audio 
{
    public static class AudioManager
    {
        private static readonly Dictionary<Audio, AudioSource[]> _audioData = new();

        #region Registration
        public static void RegisterAudio(Audio audio) 
        {
            InitializeSource(audio, out var sources);
            _audioData.Add(audio, sources);
        }

        private static void InitializeSource(Audio audio, out AudioSource[] sources)
        {
            var sourceList = new List<AudioSource>();

            for (int i = 0; i < audio.PendingSources.Length; i++)
            {
                if (!audio.PendingSources[i].TryGetComponent<AudioSource>(out var source))
                    source = audio.PendingSources[i].AddComponent<AudioSource>();

                ConfigureSource(source, audio);
                source.playOnAwake = audio.PlayOnAwake;

                sourceList.Add(source);

                if (source.playOnAwake && !source.isPlaying)
                    source.Play();
            }

            sources = sourceList.ToArray();
        }

        private static void ConfigureSource(AudioSource source, Audio audio)
        {
            source.name = audio.Name;
            source.clip = audio.Clip;
            source.volume = audio.Volume;
            source.pitch = audio.Pitch;
            source.loop = audio.Loop;

            if (audio.MixerGroup != null)
                source.outputAudioMixerGroup = audio.MixerGroup;
        }

        public static void UnregisterAudio(Audio audio)
        {
            _audioData.Remove(audio);
        }

        public static void ClearAudioData() => _audioData.Clear();
        #endregion

        #region Background Audio
        public static void Play(AudioSource source, string audioName) 
        {
            if (!TryGetAudioByName(audioName, out var result)) return;
            Play(source, result);
        }

        public static void Play(AudioSource source, AudioClip clip)
        {
            if (!TryGetAudioByClip(clip, out var result)) return;
            Play(source, result);
        }

        private static void Play(AudioSource source, Audio audio) 
        {
            Stop(source);
            ConfigureSource(source, audio);
            source.Play();
        }
        #endregion

        #region Sound Effects
        public static void PlayOneShot(AudioSource source, string audioName)
        {
            if (!TryGetAudioByName(audioName, out var result)) return;
            PlayOneShot(source, result);
        }

        public static void PlayOneShot(AudioSource source, AudioClip clip)
        {
            if (!TryGetAudioByClip(clip, out var result)) return;
            PlayOneShot(source, result);
        }

        private static void PlayOneShot(AudioSource source, Audio audio)
            => source.PlayOneShot(audio.Clip, audio.Volume);
        #endregion

        #region Pause And Stop
        public static void Pause(AudioSource source)
            => source.Pause();

        public static void Stop(AudioSource source)
            => source.Stop();
        #endregion

        #region Other API
        public static bool IsPlaying(AudioSource source)
            => source.isPlaying;

        public static AudioSource GetSourceByAudioName(string sourceName, string audioName)
        {
            var sources = GetSourcesByAudioName(audioName);
            if (sources == null) return null;

            return sources.FirstOrDefault(x => x.name == sourceName);
        }

        public static AudioSource[] GetSourcesByAudioName(string audioName) 
        {
            if (!TryGetAudioByName(audioName, out var result)) return null;

            if (!_audioData.TryGetValue(result, out var sources)) 
            {
                Debug.LogError($"AudioManager: Cannot find the sources in the target audio '{audioName}'");
                return null;
            }

            return sources;
        }

        public static AudioSource GetSourceByAudioClip(string sourceName, AudioClip clip)
        {
            var sources = GetSourcesByAudioClip(clip);
            if (sources == null) return null;

            return sources.FirstOrDefault(x => x.name == sourceName);
        }

        public static AudioSource[] GetSourcesByAudioClip(AudioClip clip)
        {
            if (!TryGetAudioByClip(clip, out var result)) return null;

            if (!_audioData.TryGetValue(result, out var sources))
            {
                Debug.LogError($"AudioManager: Cannot find the sources in the target audio '{clip}'");
                return null;
            }

            return sources;
        }
        #endregion

        #region Validation
        private static bool TryGetAudioByName(string audioName, out Audio result, bool printIfNotFound = true)
        {
            result = _audioData.Keys.FirstOrDefault(x => x.Name == audioName);

            if (result == null && printIfNotFound)
                Debug.LogError($"AudioManager: Cannot find audio {audioName}. " +
                    $"Did you registered them during initialization?");

            return result != null;
        }

        private static bool TryGetAudioByClip(AudioClip clip, out Audio result, bool printIfNotFound = true)
        {
            result = _audioData.Keys.FirstOrDefault(x => x.Clip == clip);

            if (result == null && printIfNotFound)
                Debug.LogError($"AudioManager: Cannot find audio {clip}. " +
                    $"Did you registered them during initialization?");

            return result != null;
        }
        #endregion
    }
}