// ----------------------------------------------------------------------------
// AudioSettings.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 12/08/2022
// ----------------------------------------------------------------------------

using System;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// A basic settings struct for the most common settings on a source...
    /// </summary>
    [Serializable]
    public struct AudioSettings
    {
        public bool useVariance;                // Should the clip use variance...
        public float volume;                    // The volume to play at...
        public float pitch;                     // The pitch to play...
        public bool loop;                       // Should the clip loop...
        public int priority;                    // The priority of the clip...
        public AudioMixerGroup mixerGroup;      // The mixer group to play in...
    }
}