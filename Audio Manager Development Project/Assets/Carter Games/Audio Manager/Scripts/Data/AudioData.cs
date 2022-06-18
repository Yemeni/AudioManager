using System;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public class AudioData
    {
        public string key;
        public AudioClip value;
        public float baseVolume = 1f;
        public float basePitch = 1f;
        

        public AudioData(string key, AudioClip value)
        {
            this.key = key;
            this.value = value;
            baseVolume = 1f;
            basePitch = 1f;
        }
    }
}