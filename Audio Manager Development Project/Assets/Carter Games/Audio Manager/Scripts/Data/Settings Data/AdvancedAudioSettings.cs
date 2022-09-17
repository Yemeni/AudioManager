using System;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// A more in-depth settings struct for more advanced use...
    /// </summary>
    [Serializable]
    public struct AdvancedAudioSettings
    {
        //
        // Basic Settings
        //
        
        
        public bool useVariance;                // Should the clip use variance...
        public float volume;                    // The volume to play at...
        public float pitch;                     // The pitch to play...
        public bool loop;                       // Should the clip loop...
        public int priority;                    // The priority of the clip...
        public AudioMixerGroup mixerGroup;      // The mixer group to play in...
        
        
        //
        // Extra Settings
        //
        
        
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
}