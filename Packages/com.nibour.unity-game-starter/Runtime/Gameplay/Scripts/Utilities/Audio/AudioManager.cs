using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Audio Play API
        public static void Play(string audioName) 
        {
            if (!TryGetAudioSourceByName(audioName, out var result)) return;

            Stop(audioName);
            result.Play();
        }

        public static void PlayOneShot(string audioName, AudioClip clip, float volume = 1)
        {
            if (!TryGetAudioSourceByName(audioName, out var result)) return;

            result.PlayOneShot(clip, volume);
        }
        #endregion

        #region Audio State
        public static bool IsPlaying(string audioName)
        {
            if (!TryGetAudioSourceByName(audioName, out var result)) return false;

            return result.isPlaying;
        }

        public static void Pause(string audioName) 
        {
            if (!TryGetAudioSourceByName(audioName, out var result)) return;

            result.Pause();
        }

        public static void Stop(string audioName)
        {
            if (!TryGetAudioSourceByName(audioName, out var result)) return;

            result.Stop();
        }
        #endregion

        #region Get Audio Properties
        public static bool TryGetAudioSourceByName(
            string audioName, out AudioSource result, bool printIfNotFound = true)
        {
            result = null;

            if (!TryGetAudioByName(audioName, out var audio, printIfNotFound)) return false;

            result = audio.Source;

            return result != null;
        }

        public static AudioType GetAudioTypeByName(string audioName, bool printIfNotFound = true)
        {
            if (!TryGetAudioByName(audioName, out var audio, printIfNotFound))
                throw new Exception();

            return audio.Type;
        }

        private static bool TryGetAudioByName(string audioName, out Audio result, bool printIfNotFound = true)
        {
            result = _audioData.FirstOrDefault(x => x.Name == audioName);

            if (result == null && printIfNotFound)
                Debug.LogError($"AudioManager: Cannot find audio {audioName}. " +
                    $"Did you registered them during initialization?");

            return result != null;
        }
        #endregion
    }
}