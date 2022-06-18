using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public struct AudioSettings
    {
        public float volume;
        public float pitch;
        public bool loop;
        public int priority;
        public AudioMixerGroup mixerGroup;
    }
    
    [Serializable]
    public struct AdvancedAudioSettings
    {
        // Basic Settings
        public bool useVariance;
        public float volume;
        public float pitch;
        public bool loop;
        public int priority;
        public AudioMixerGroup mixerGroup;
        
        // Extra Settings
        public bool? bypassEffects;
        public bool? bypassListenerEffects;
        public bool? bypassReverbZones;
        public float? stereoPan;
        public float? reverbZoneMix;
        public float? spread;
        public float? dopplerLevel;
        public AudioRolloffMode rollOffMode;
        public float? minDistance;
        public float? maxDistance;
    }
    
    public static class AudioHandler
    {
        private static AudioManagerSettings CachedSettings;


        private static AudioManagerSettings Settings
        {
            get 
            {
                if (CachedSettings == null)
                    CachedSettings = Resources.Load("Audio Manager/Audio Manager Settings") as AudioManagerSettings;

                return CachedSettings;
            }
        }


        public static AudioClipPlayer SetupAudioSource(AudioClip clip, AudioSettings settings)
        {
            AudioPool.GetPrefabFromPool(out var source);
            
            if (source == null)
            {
                AmLog.ErrorWithCode(1, "Unable to find Audio Prefab Manager on prefab object, please ensure the Audio Prefab Manager is on the prefab object.");
                return null;
            }

            var audioSource = source.Source;

            audioSource.clip = clip;
            audioSource.volume = settings.volume;
            audioSource.pitch = settings.pitch;
            audioSource.loop = settings.loop;
            audioSource.priority = settings.priority;
            audioSource.outputAudioMixerGroup = settings.mixerGroup;

            return source;
        }
        
        public static AudioClipPlayer SetupAudioSource(AudioClip clip, AdvancedAudioSettings settings)
        {
            AudioPool.GetPrefabFromPool(out var source);
            
            if (source == null)
            {
                AmLog.ErrorWithCode(1, "Unable to find Audio Prefab Manager on prefab object, please ensure the Audio Prefab Manager is on the prefab object.");
                return null;
            }

            var audioSource = source.Source;

            // Basic
            audioSource.clip = clip;
            audioSource.volume = settings.useVariance ? Mathf.Clamp01(settings.volume + Settings.variantVolume) : settings.volume;
            audioSource.pitch = settings.useVariance ? Mathf.Clamp(settings.pitch + Settings.variantPitch, -3f, 3f) : settings.pitch;
            audioSource.loop = settings.loop;
            audioSource.priority = settings.priority;
            audioSource.outputAudioMixerGroup = settings.mixerGroup;
            
            // Advanced
            audioSource.bypassEffects = settings.bypassEffects ?? false;
            audioSource.bypassListenerEffects = settings.bypassListenerEffects ?? false;
            audioSource.bypassReverbZones = settings.bypassReverbZones ?? false;
            audioSource.panStereo = settings.stereoPan ?? 0f;
            audioSource.reverbZoneMix = settings.reverbZoneMix ?? 1f;
            audioSource.rolloffMode = settings.rollOffMode;
            audioSource.spread = settings.spread ?? 0f;
            audioSource.dopplerLevel = settings.dopplerLevel ?? 1f;
            audioSource.minDistance = settings.minDistance ?? 1f;
            audioSource.maxDistance = settings.maxDistance ?? 500f;

            return source;
        }
        

        public static AudioSettings GenerateSettings(AudioMixerGroup mixerGroup, float? volume, float? pitch, int? priority, bool? loop)
        {
            return new AudioSettings()
            {
                volume = volume ?? 1f,
                pitch = pitch ?? 1f,
                loop = loop ?? false,
                priority = priority ?? 128,
                mixerGroup = mixerGroup,
            };
        }

        public static AudioClipInfo GenerateClipInfo(string id, AudioClipPlayer source)
        {
            source.ClipInfo.SetClipId(id);
            return source.ClipInfo;
        }

        public static AudioClipInfo ClipPlayer(AudioClipPlayer source)
        {
            source.Source.Play();
            AudioEvents.OnClipPlayed.Raise(source.ClipInfo);
            return source.ClipInfo;
        }
        
        public static AudioClipInfo ClipPlayer(AudioClipPlayer source, float delay)
        {
            source.Source.PlayDelayed(delay);
            AudioEvents.OnClipPlayed.Raise(source.ClipInfo);
            return source.ClipInfo;
        }
    }
}