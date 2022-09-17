using System;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// The data for an entry in the library...
    /// </summary>
    [Serializable]
    public class AudioData
    {
        //
        //  Fields
        //
        
        
        public string key;              // The name of the clip...
        public AudioClip value;         // The audioClip...
        public float baseVolume = 1f;   // The base volume of the clip...
        public float basePitch = 1f;    // The base pitch of the clip...
        public AudioLabel label;        // The label for the clip...
        

        //
        //  Constructors
        //
        
        
        /// <summary>
        /// Creates a new audio data with the key & clip entered...
        /// </summary>
        /// <param name="key">The key to use...</param>
        /// <param name="value">The clip data to use...</param>
        public AudioData(string key, AudioClip value)
        {
            this.key = key;
            this.value = value;
            baseVolume = 1f;
            basePitch = 1f;
            label = AudioLabel.Sfx;
        }
    }
}