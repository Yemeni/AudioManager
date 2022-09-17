using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Handles The setup & playing of clip players when called for...
    /// </summary>
    public static class AudioHandler
    {
        //
        //  Fields
        //
        
        
        private static AudioManagerSettings _cachedSettings;                // A cache of the settings asset for use...


        //
        //  Properties
        //
        
        
        /// <summary>
        /// Gets the settings asset if the cache does have it already...
        /// </summary>
        private static AudioManagerSettings Settings
        {
            get
            {
                if (_cachedSettings == null)
                    _cachedSettings = AssetAccessor.GetAsset<AudioManagerSettings>();

                return _cachedSettings;
            }
        }

        
        //
        //  Methods
        //
        
        
        /// <summary>
        /// Sets up an audio clip player for use...
        /// </summary>
        /// <param name="clip">The audio clip to play...</param>
        /// <param name="settings">The settings to apply...</param>
        /// <returns>A setup clip player ready for use...</returns>
        public static AudioClipPlayer SetupClipPlayer(AudioClip clip, AudioSettings settings)
        {
            var clipPlayer = AudioPool.GetPrefabFromPool();
            
            if (clipPlayer == null)
            {
                AmLog.ErrorWithCode(1, "Unable to find Audio Prefab Manager on prefab object, please ensure the Audio Prefab Manager is on the prefab object.");
                return null;
            }

            var audioSource = clipPlayer.AudioSource;

            audioSource.clip = clip;
            audioSource.volume = settings.volume;
            audioSource.pitch = settings.pitch;
            audioSource.loop = settings.loop;
            audioSource.priority = settings.priority;
            audioSource.outputAudioMixerGroup = settings.mixerGroup;

            clipPlayer.ClipInfo = new AudioClipInfo(audioSource);

            return clipPlayer;
        }
        
        
        /// <summary>
        /// Sets up an audio clip player for use...
        /// </summary>
        /// <param name="clip">The audio clip to play...</param>
        /// <param name="settings">The settings to apply...</param>
        /// <returns>A setup clip player ready for use...</returns>
        public static AudioClipPlayer SetupClipPlayer(AudioClip clip, AdvancedAudioSettings settings)
        {
            var clipPlayer = AudioPool.GetPrefabFromPool();
            
            if (clipPlayer == null)
            {
                AmLog.ErrorWithCode(1, "Unable to find Audio Prefab Manager on prefab object, please ensure the Audio Prefab Manager is on the prefab object.");
                return null;
            }

            var audioSource = clipPlayer.AudioSource;

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

            clipPlayer.ClipInfo = new AudioClipInfo(audioSource);
            
            return clipPlayer;
        }
        

        /// <summary>
        /// Generates a settings asset based on the values entered...
        /// </summary>
        /// <param name="mixerGroup"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <param name="priority"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Plays a clip...
        /// </summary>
        /// <param name="clipPlayer">The player to use...</param>
        /// <returns>The clip info for the now playing clip...</returns>
        public static AudioClipInfo PlayClip(AudioClipPlayer clipPlayer)
        {
            clipPlayer.SetData(clipPlayer.ClipInfo);
            clipPlayer.AudioSource.Play();
            AudioEvents.OnClipPlayed.Raise(clipPlayer.ClipInfo);
            return clipPlayer.ClipInfo;
        }
        
        
        /// <summary>
        /// Plays a clip with a delay...
        /// </summary>
        /// <param name="clipPlayer">The player to use...</param>
        /// <param name="delay">the delay to playing the clip...</param>
        /// <returns>The clip info for the now playing clip...</returns>
        public static AudioClipInfo PlayClip(AudioClipPlayer clipPlayer, float delay)
        {
            clipPlayer.AudioSource.PlayDelayed(delay);
            AudioEvents.OnClipPlayed.Raise(clipPlayer.ClipInfo);
            return clipPlayer.ClipInfo;
        }
    }
}