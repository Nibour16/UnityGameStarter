using System.Collections.Generic;
using UnityEngine;

namespace UnityGameStarter.Gameplay.Audio 
{
    public static class AudioManager
    {
        private static readonly List<Audio> _audioData = new();

        #region Registration
        public static void RegisterAudio(Audio audio) => _audioData.Add(audio);

        public static void UnregisterAudio(Audio audio) => _audioData.Remove(audio);

        public static void ClearAudioData() => _audioData.Clear();
        #endregion

        #region Background Audio
        public static void Play(ref AudioSource source, string audioName) 
        {
            if (!TryGetAudioByName(audioName, out var result)) return;
            Play(ref source, result);
        }

        public static void Play(ref AudioSource source, AudioClip clip)
        {
            if (!TryGetAudioByClip(clip, out var result)) return;
            Play(ref source, result);
        }

        private static void Play(ref AudioSource source, Audio audio) 
        {
            Stop(source);
            ConfigureSource(ref source, audio);
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

        public static void ConfigureSource(ref AudioSource source, Audio audio)
        {
            source.name = audio.Name;
            source.clip = audio.Clip;
            source.volume = audio.Volume;
            source.pitch = audio.Pitch;
            source.loop = audio.Loop;

            if (audio.MixerGroup != null)
                source.outputAudioMixerGroup = audio.MixerGroup;
        }

        public static void InitializeSource(ref AudioSource source, Audio audio) 
        {
            source.playOnAwake = audio.PlayOnAwake;
            ConfigureSource(ref source, audio);
        }
        #endregion

        #region Validation
        private static bool TryGetAudioByName(string audioName, out Audio result, bool printIfNotFound = true)
        {
            result = _audioData.Find(x => x.Name == audioName);

            if (result == null && printIfNotFound)
                Debug.LogError($"Cannot find audio {audioName}. Did you registered them during initialization?");

            return result != null;
        }

        private static bool TryGetAudioByClip(AudioClip clip, out Audio result, bool printIfNotFound = true)
        {
            result = _audioData.Find(x => x.Clip == clip);

            if (result == null && printIfNotFound)
                Debug.LogError($"Cannot find audio {clip}. Did you registered them during initialization?");

            return result != null;
        }
        #endregion
    }
}